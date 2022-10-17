using System.Collections.Generic;
using System.Threading.Tasks;
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

        private void Awake()
        {
            CreateBoard();
            boardView.Setup(boardSettings, _board);

            IsMatchDetected(out var matches);
            OnMove(matches);
        }

        private void CreateBoard()
        {
            var size = boardSettings.boardSize;
            _board = new BoardCell[size.x, size.y];

            for (var i = 0; i < size.x; i++)
            {
                for (var j = 0; j < size.y; j++)
                {
                    // todo copy instead
                    _board[i, j] = boardSettings.IsValid
                        ? boardSettings.initialLayout[i + j]
                        : _board[i, j] = new BoardCell { chip = GetRandomChip() };
                    _board[i, j].index = new Vector2Int(i, j);
                }
            }
        }

        private void OnSwap(Vector2Int sourcePosition, Vector2Int destinationPosition)
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

            DoSwap(cellSource, cellDestination);
            if (IsMatchDetected(out var matches))
            {
                OnMove(matches);
            }
            else if (!boardSettings.allowNonMatchSwipe)
            {
                DoSwap(cellDestination, cellSource);
            }
        }

        private void DoSwap(BoardCell cellSource, BoardCell cellDestination)
        {
            cellSource.chip.OnSwap?.Invoke(cellDestination.index);
            cellSource.chip.OnSwap?.Invoke(cellSource.index);
            (cellSource.chip, cellDestination.chip) = (cellDestination.chip, cellSource.chip);
        }

        private void OnMove(List<BoardCell> boardCells)
        {
            while (boardCells.Count > 0)
            {
                MatchChips(boardCells);
                IsMatchDetected(out boardCells);
            }
        }

        private async void MatchChips(List<BoardCell> boardCells)
        {
            foreach (var cell in boardCells)
            {
                cell.chip.ApplyEffect();
                await UniTask.Delay(100);
                cell.chip.OnEffect?.Invoke();
                cell.chip = null;
            }
            
            // spawn new chips
        }

        private bool IsMatchDetected(out List<BoardCell> matches)
        {
            matches = new List<BoardCell>();
            for (var i = 0; i < boardSettings.boardSize.x; i++)
            {
                for (var j = 0; j < boardSettings.boardSize.y; j++)
                {
                    if (CheckMatch(_board[i,j]))
                    {
                        matches.Add(_board[i,j]);
                    }
                }
            }
            
            Debug.Log(matches.Count);
            return matches.Count > 0;
        }

        private bool CheckMatch(BoardCell source)
        {
            var count = 0;
            var sourceColor = (source.chip as SimpleColorChip)?.color;

            for (var i = source.index.x - 1; i >= 0; i--)
            {
                if (!CompareColors(i, source.index.y, sourceColor)) break;
                count++;
            }

            for (var i = source.index.x + 1; i < _board.GetLength(0); i++)
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

            for (var j = source.index.y + 1; j < _board.GetLength(1); j++)
            {
                if (!CompareColors(source.index.x, j, sourceColor)) break;
                count++;
            }
              
            return count > 1;
        }

        private bool CompareColors(int x, int y, Color sourceColor)
        {
            var toCompare = _board[x, y];
            return (toCompare.chip as SimpleColorChip)?.color.name == sourceColor.name;
        }

        private BoardElement GetRandomChip()
        {
            return chips.Random(_random);
        }
    }
}