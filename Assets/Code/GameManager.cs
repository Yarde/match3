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
                }
            }
        }

        private BoardElement GetRandomChip()
        {
            return chips.Random(_random);
        }
    }
}