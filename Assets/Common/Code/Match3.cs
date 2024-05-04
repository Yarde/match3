using System;
using System.Collections.Generic;
using Common.Code.Model;
using Common.Code.Model.Chips;
using Common.Code.Utils;
using Common.Code.View;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Common.Code
{
    public class Match3
    {
        public event Action OnGameStarted;
        public event Action OnGameEnded;
        public event Action OnMove;
        public event Action<int> OnMatch;
        
        private BoardCell[,] _board;
        private bool _busy;
        private BoardSettings _boardSettings;
        private BoardView _boardView;

        public async UniTask StartGame(BoardSettings boardSettings)
        {
            _boardSettings = boardSettings;
            _boardView = CreateBoard();
            await _boardView.Setup(boardSettings, _board);
            foreach (var chip in _boardView.Prefabs)
            {
                SubscribeToUserActions(chip);
            }

            OnGameStarted?.Invoke();
            await ClearInitialMatches();
        }
        
        public async UniTask EndGame()
        {
            await UniTask.WaitWhile(() => _busy);
            _busy = true;
            Object.Destroy(_boardView.gameObject);
            OnGameEnded?.Invoke();
        }

        private async UniTask ClearInitialMatches()
        {
            _busy = true;
            await UniTask.Delay(200);
            IsMatchDetected(out var matches);
            await OnMatchPossible(matches);
            _busy = false;
        }

        private void SubscribeToUserActions(ChipView chip)
        {
            chip.OnSwapped += OnSwap;
            chip.OnClicked += OnClick;
        }

        private BoardView CreateBoard()
        {
            var size = _boardSettings.boardSize;
            _board = new BoardCell[size.x, size.y];

            for (var i = 0; i < size.x; i++)
            {
                for (var j = 0; j < size.y; j++)
                {
                    _board[i, j] = new BoardCell
                    {
                        chip = Object.Instantiate(_boardSettings.generatorData.GetChip()),
                        Index = new Vector2Int(i, j)
                    };
                }
            }
            
            return Object.Instantiate(_boardSettings.boardViewPrefab);
        }

        private void OnSwap(Vector2Int sourcePosition, Vector2Int destinationPosition)
        {
            if (_busy)
            {
                return;
            }

            TryToSwap(sourcePosition, destinationPosition).Forget();
        }

        private void OnClick(Vector2Int source)
        {
            if (_busy)
            {
                return;
            }

            var clickedCell = _board[source.x, source.y];
            if (!clickedCell.chip.isClickable)
            {
                return;
            }
            
            _busy = true;
            var matches = new HashSet<BoardCell> { clickedCell };
            OnMatchPossible(matches).ContinueWith(() => _busy = false);
            OnMove?.Invoke();
        }

        private void TryToGetEffect(BoardCell boardCell, ISet<BoardCell> matches)
        {
            var effectPredicate = boardCell.chip.GetEffectPredicate();
            if (effectPredicate != null)
            {
                GetEffectTargets(boardCell, effectPredicate, matches);
            }
        }

        private void GetEffectTargets(BoardCell source, Func<BoardCell, BoardCell, bool> effectPredicate,
            ISet<BoardCell> matches)
        {
            for (var i = 0; i < _board.GetLength(0); i++)
            {
                for (var j = 0; j < _board.GetLength(1); j++)
                {
                    if (effectPredicate(source, _board[i, j]))
                    {
                        matches.Add(_board[i, j]);
                    }
                }
            }
        }

        private async UniTaskVoid TryToSwap(Vector2Int sourcePosition, Vector2Int destinationPosition)
        {
            _busy = true;
            var cellSource = _board[sourcePosition.x, sourcePosition.y];
            if (!cellSource.chip.isSwappable)
            {
                Debug.Log("Chip Not Swappable!");
                _busy = false;
                return;
            }

            if (_boardSettings.boardSize.InBounds2D(destinationPosition))
            {
                Debug.Log("Destination Out Of Bounds!");
                _busy = false;
                return;
            }

            var cellDestination = _board[destinationPosition.x, destinationPosition.y];
            if (!cellDestination.chip.isSwappable)
            {
                Debug.Log("Destination not Swappable!");
                _busy = false;
                return;
            }

            await DoSwap(cellSource, cellDestination);
            if (IsMatchDetected(out var matches))
            {
                await OnMatchPossible(matches);
                OnMove?.Invoke();
            }
            else{
                if (!_boardSettings.allowNonMatchSwipe)
                {
                    await DoSwap(cellDestination, cellSource);
                }
                else
                {
                    OnMove?.Invoke();
                }
            }

            _busy = false;
        }

        private async UniTask DoSwap(BoardCell cellSource, BoardCell cellDestination)
        {
            var moveSource = new Move(cellSource.Index, cellDestination.Index, 0f);
            var moveSourceTask = cellSource.chip.OnMove.Invoke(moveSource);
            var moveDestination = new Move(cellDestination.Index, cellSource.Index, 0f);
            var moveDestinationTask = cellDestination.chip.OnMove.Invoke(moveDestination);
            
            await UniTask.WhenAll(moveSourceTask, moveDestinationTask);
            (cellSource.chip, cellDestination.chip) = (cellDestination.chip, cellSource.chip);
        }

        private async UniTask OnMatchPossible(HashSet<BoardCell> matches)
        {
            while (matches.Count > 0)
            {
                GetAffectedCells(matches);
                await MatchChips(matches);
                IsMatchDetected(out matches);
            }
        }

        private void GetAffectedCells(HashSet<BoardCell> matches)
        {
            while (true)
            {
                var newMatches = new HashSet<BoardCell>();
                foreach (var cell in matches)
                {
                    TryToGetEffect(cell, newMatches);
                }

                var oldMatches = matches.Count;
                matches.UnionWith(newMatches);
                if (oldMatches < matches.Count)
                {
                    continue;
                }

                break;
            }
        }

        private async UniTask MatchChips(HashSet<BoardCell> matches)
        {
            var awaitable = new List<UniTask>();

            foreach (var cell in matches)
            {
                if (cell.chip.OnEffect != null)
                {
                    awaitable.Add(cell.chip.OnEffect.Invoke());
                }
            }

            OnMatch?.Invoke(matches.Count);
            await UniTask.WhenAll(awaitable);

            foreach (var cell in matches)
            {
                cell.chip = null;
            }

            await SpawnNewChips();
        }

        private async UniTask SpawnNewChips()
        {
            var awaitable = new List<UniTask>();

            for (var i = 0; i < _board.GetLength(0); i++)
            {
                var queueOffset = 0;
                for (var j = 0; j < _board.GetLength(1); j++)
                {
                    var cell = _board[i, j];
                    if (cell.chip == null)
                    {
                        var (newChip, move) = GetNewChip(i, j, queueOffset * 0.1f);
                        cell.chip = newChip;
                        awaitable.Add(move);
                        queueOffset++;
                    }
                }
            }

            await UniTask.WhenAll(awaitable);
        }

        private (BoardElement, UniTask) GetNewChip(int i, int j, float delay)
        {
            return TryToGetCell(i, j, delay, out var cell) 
                ? cell 
                : CreateNewCell(i, j, delay);
        }

        private (BoardElement, UniTask) CreateNewCell(int i, int j, float delay)
        {
            var newChip = Object.Instantiate(_boardSettings.generatorData.GetChip());
            var chipView = _boardView.CreateNewChip(i, _board.GetLength(1), newChip, _boardSettings);
            SubscribeToUserActions(chipView);
            var move = new Move(new Vector2Int(i, _board.GetLength(1)), new Vector2Int(i, j), delay);
            var newMove = newChip.OnMove.Invoke(move);
            return (newChip, newMove);
        }

        private bool TryToGetCell(int i, int j, float delay, out (BoardElement, UniTask) valueTuple)
        {
            for (var k = j + 1; k < _board.GetLength(1); k++)
            {
                var chip = _board[i, k].chip;
                if (chip != null)
                {
                    var move = new Move(new Vector2Int(i, k), new Vector2Int(i, j), delay);
                    var moveTask = chip.OnMove.Invoke(move);
                    _board[i, k].chip = null;
                    {
                        valueTuple = (chip, moveTask);
                        return true;
                    }
                }
            }

            valueTuple = default;
            return false;
        }

        private bool IsMatchDetected(out HashSet<BoardCell> matches)
        {
            matches = new HashSet<BoardCell>();
            for (var i = 0; i < _boardSettings.boardSize.x; i++)
            {
                for (var j = 0; j < _boardSettings.boardSize.y; j++)
                {
                    if (_board[i, j].CheckMatch(_boardSettings, _board))
                    {
                        matches.Add(_board[i, j]);
                    }
                }
            }

            return matches.Count > 0;
        }
    }
}