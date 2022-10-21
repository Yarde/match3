using System;
using System.Collections.Generic;
using System.Linq;
using Code.ChipGenerator;
using Code.Model;
using Code.Model.Chips;
using Code.Utils;
using Code.View;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BoardSettings boardSettings;
        [SerializeField] private BoardView boardView;
        [SerializeField] private ChipGeneratorBase chipGeneratorBase;

        private BoardCell[,] _board;
        private bool _busy;

        private async UniTaskVoid Awake()
        {
            CreateBoard();
            await boardView.Setup(boardSettings, _board);
            foreach (var chip in boardView.Prefabs)
            {
                SubscribeToUserActions(chip);
            }

            await UniTask.Delay(200);

            IsMatchDetected(out var matches);
            await OnMatchPossible(matches);
        }

        private void SubscribeToUserActions(ChipView chip)
        {
            chip.OnSwapped += OnSwap;
            chip.OnClicked += OnClick;
        }

        private void CreateBoard()
        {
            var size = boardSettings.boardSize;
            _board = new BoardCell[size.x, size.y];

            for (var i = 0; i < size.x; i++)
            {
                for (var j = 0; j < size.y; j++)
                {
                    _board[i, j] = new BoardCell
                    {
                        chip = Instantiate(boardSettings.isValid
                            ? boardSettings.initialLayout[i + j]
                            : chipGeneratorBase.GetChip()),
                        Index = new Vector2Int(i, j)
                    };
                }
            }
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
            var matches = new HashSet<BoardCell> { clickedCell };
            TryToGetEffect(clickedCell, matches);
        }

        private void TryToGetEffect(BoardCell boardCell, ISet<BoardCell> matches)
        {
            var effectPredicate = boardCell.chip.GetEffectPredicate();
            if (effectPredicate != null)
            {
                GetEffectTargets(effectPredicate, matches);
            }
        }

        private void GetEffectTargets(Func<BoardCell, bool> effectPredicate, ISet<BoardCell> matches)
        {
            for (var i = 0; i < _board.GetLength(0); i++)
            {
                for (var j = 0; j < _board.GetLength(1); j++)
                {
                    if (effectPredicate(_board[i, j]))
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

            if (boardSettings.boardSize.InBounds2D(destinationPosition))
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
            }
            else if (!boardSettings.allowNonMatchSwipe)
            {
                await DoSwap(cellDestination, cellSource);
            }

            _busy = false;
        }

        private async UniTask DoSwap(BoardCell cellSource, BoardCell cellDestination)
        {
            await UniTask.WhenAll(
                cellSource.chip.OnMove.Invoke(cellSource.Index, cellDestination.Index),
                cellDestination.chip.OnMove.Invoke(cellDestination.Index, cellSource.Index));
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
            var newMatches = new HashSet<BoardCell>();
            foreach (var cell in matches)
            {
                TryToGetEffect(cell, newMatches);
            }

            matches.UnionWith(newMatches);
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
                for (var j = 0; j < _board.GetLength(1); j++)
                {
                    var cell = _board[i, j];
                    if (cell.chip == null)
                    {
                        var (newChip, move) = GetNewChip(i, j);
                        cell.chip = newChip;
                        awaitable.Add(move);
                    }
                }
            }

            await UniTask.WhenAll(awaitable);
        }

        private (BoardElement, UniTask) GetNewChip(int i, int j)
        {
            return TryToGetCell(i, j, out var cell) ? cell : CreateNewCell(i, j);
        }

        private (BoardElement, UniTask) CreateNewCell(int i, int j)
        {
            var newChip = Instantiate(chipGeneratorBase.GetChip());
            var chipView = boardView.CreateNewChip(i, _board.GetLength(1), newChip, boardSettings);
            SubscribeToUserActions(chipView);
            var newMove = newChip.OnMove.Invoke(new Vector2Int(i, _board.GetLength(1)), new Vector2Int(i, j));
            return (newChip, newMove);
        }

        private bool TryToGetCell(int i, int j, out (BoardElement, UniTask) valueTuple)
        {
            for (var k = j + 1; k < _board.GetLength(0); k++)
            {
                var chip = _board[i, k].chip;
                if (chip != null)
                {
                    var move = chip.OnMove.Invoke(new Vector2Int(i, k), new Vector2Int(i, j));
                    _board[i, k].chip = null;
                    {
                        valueTuple = (chip, move);
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
            for (var i = 0; i < boardSettings.boardSize.x; i++)
            {
                for (var j = 0; j < boardSettings.boardSize.y; j++)
                {
                    if (_board[i, j].CheckMatch(boardSettings, _board))
                    {
                        matches.Add(_board[i, j]);
                    }
                }
            }

            return matches.Count > 0;
        }
    }
}