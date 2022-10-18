using System.Collections.Generic;
using Code.Model;
using Code.Model.Chips;
using Code.Utils;
using Code.View;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Color = Code.Model.Color;
using Random = System.Random;

namespace Code
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BoardSettings boardSettings;
        [SerializeField] private BoardView boardView;
        [SerializeField] private List<SimpleColorChip> chips;

        private BoardCell[,] _board;
        private readonly Random _random = new();

        private async UniTaskVoid Awake()
        {
            CreateBoard();
            boardView.Setup(boardSettings, _board);

            await UniTask.Delay(1000);

            IsMatchDetected(out var matches);
            await OnMove(matches);
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
                        chip = Instantiate(boardSettings.IsValid
                            ? boardSettings.initialLayout[i + j]
                            : GetRandomChip()),
                        index = new Vector2Int(i, j)
                    };
                }
            }
        }

        private async UniTask OnSwap(Vector2Int sourcePosition, Vector2Int destinationPosition)
        {
            var cellSource = _board[sourcePosition.x, sourcePosition.y];
            if (!cellSource.chip.isSwappable)
            {
                Debug.Log("Chip Not Swappable!");
                return;
            }

            if (boardSettings.boardSize.InBounds2D(destinationPosition))
            {
                Debug.Log("Destination Out Of Bounds!");
                return;
            }

            var cellDestination = _board[destinationPosition.x, destinationPosition.y];
            if (!cellDestination.chip.isSwappable)
            {
                Debug.Log("Destination not Swappable!");
                return;
            }

            await DoSwap(cellSource, cellDestination);
            if (IsMatchDetected(out var matches))
            {
                await OnMove(matches);
            }
            else if (!boardSettings.allowNonMatchSwipe)
            {
                await DoSwap(cellDestination, cellSource);
            }
        }

        private async UniTask DoSwap(BoardCell cellSource, BoardCell cellDestination)
        {
            await UniTask.WhenAll(
                cellSource.chip.OnMove.Invoke(cellSource.index, cellDestination.index),
                cellSource.chip.OnMove.Invoke(cellDestination.index, cellSource.index));
            (cellSource.chip, cellDestination.chip) = (cellDestination.chip, cellSource.chip);
        }

        private async UniTask OnMove(List<BoardCell> boardCells)
        {
            while (boardCells.Count > 0)
            {
                await MatchChips(boardCells);
                IsMatchDetected(out boardCells);
            }
        }

        private async UniTask MatchChips(List<BoardCell> boardCells)
        {
            var awaitable = new List<UniTask>();

            foreach (var cell in boardCells)
            {
                cell.chip.ApplyEffect();
                if (cell.chip.OnEffect != null)
                {
                    awaitable.Add(cell.chip.OnEffect.Invoke());
                }
            }

            await UniTask.WhenAll(awaitable);

            foreach (var cell in boardCells)
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
            for (var k = j + 1; k < _board.GetLength(0); k++)
            {
                var chip = _board[i, k].chip;
                if (chip != null)
                {
                    var move = chip.OnMove.Invoke(new Vector2Int(i, k), new Vector2Int(i, j));
                    _board[i, k].chip = null;
                    return (chip, move);
                }
            }

            var newChip = Instantiate(GetRandomChip());
            boardView.CreateNewChip(i, _board.GetLength(1), newChip);
            var newMove = newChip.OnMove.Invoke(new Vector2Int(i, _board.GetLength(1)), new Vector2Int(i, j));
            return (newChip, newMove);
        }

        private bool IsMatchDetected(out List<BoardCell> matches)
        {
            matches = new List<BoardCell>();
            for (var i = 0; i < boardSettings.boardSize.x; i++)
            {
                for (var j = 0; j < boardSettings.boardSize.y; j++)
                {
                    if (CheckMatch(_board[i, j]))
                    {
                        matches.Add(_board[i, j]);
                    }
                }
            }

            return matches.Count > 0;
        }

        private bool CheckMatch(BoardCell source)
        {
            var count = 0;
            var sourceColor = (source.chip as SimpleColorChip)?.color;
            if (sourceColor == null)
            {
                return false;
            }

            for (var i = source.index.x - 1; i >= 0; i--)
            {
                if (!CompareColors(i, source.index.y, sourceColor)) break;
                count++;
            }

            for (var i = source.index.x + 1; i < _board.GetLength(1); i++)
            {
                if (!CompareColors(i, source.index.y, sourceColor)) break;
                count++;
            }

            if (count > 1)
            {
                return true;
            }

            count = 0;
            for (var j = source.index.y - 1; j >= 0; j--)
            {
                if (!CompareColors(source.index.x, j, sourceColor)) break;
                count++;
            }

            for (var j = source.index.y + 1; j < _board.GetLength(0); j++)
            {
                if (!CompareColors(source.index.x, j, sourceColor)) break;
                count++;
            }

            return count > 1;
        }

        private bool CompareColors(int x, int y, Color sourceColor)
        {
            var toCompare = _board[x, y];
            if (toCompare.chip != null)
            {
                var chip = toCompare.chip as SimpleColorChip;
                if (chip != null && chip.color != null)
                {
                    return chip.color.name == sourceColor.name;
                }

                return false;
            }

            return false;
        }

        private BoardElement GetRandomChip()
        {
            return chips.Random(_random);
        }
    }
}