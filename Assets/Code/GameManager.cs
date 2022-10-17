using System.Collections.Generic;
using Code.Model;
using Code.Model.Chips;
using Code.Utils;
using Code.View;
using UnityEngine;
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
            OnMove();
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
            if (IsMatchDetected())
            {
                OnMove();
            }
            else if (!boardSettings.allowNonMatchSwipe)
            {
                DoSwap(cellDestination, cellSource);
            }
        }

        private void DoSwap(BoardCell cellSource, BoardCell cellDestination)
        {
            cellSource.chip.OnSwap.Invoke(cellDestination.index);
            cellSource.chip.OnSwap.Invoke(cellSource.index);
            (cellSource.chip, cellDestination.chip) = (cellDestination.chip, cellSource.chip);
        }

        private void OnMove()
        {
            while (IsMatchDetected())
            {
                MatchChips();
            }
        }

        private void MatchChips()
        {
            // destroy matches, and act/generate more
        }

        private bool IsMatchDetected()
        {
            // detect 3 or more in row 
            return false;
        }

        private BoardElement GetRandomChip()
        {
            return chips.Random(_random);
        }
    }
}