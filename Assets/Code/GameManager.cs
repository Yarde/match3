using System.Collections.Generic;
using Code.Model;
using Code.Model.Chips;
using Code.Utils;
using UnityEngine;

namespace Code
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private List<SimpleColorChip> chips;

        private BoardCell[,] _board;

        private void Awake()
        {
            CreateBoard();
        }

        private void CreateBoard()
        {
            var size = _boardSettings.boardSize;
            _board = new BoardCell[size.x, size.y];


            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    // todo copy instead
                    _board[i, j] = _boardSettings.IsValid
                        ? _boardSettings.initialLayout[i + j]
                        : _board[i, j] = new BoardCell { chip = GetRandomChip() };
                }
            }
        }

        private BoardElement GetRandomChip()
        {
            return chips.Random();
        }
    }
}