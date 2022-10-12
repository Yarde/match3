using System;
using UnityEngine;
using UnityEngine.Assertions;
using Vector2 = System.Numerics.Vector2;

namespace Code.Model
{
    public class BoardSettings : ScriptableObject
    {
        public Vector2Int boardSize;
        public BoardLayout boardLayout;

        private void OnValidate()
        {
            Assert.IsTrue(boardLayout.cells.Length == boardSize.x);
            Assert.IsTrue(boardLayout.cells.GetLength(0) == boardSize.y);
        }
    }

    public class BoardLayout : ScriptableObject
    {
        public BoardCell[][] cells;
    }

    [Serializable]
    public class BoardCell
    {
        public BoardElement chip;
        // optional extra modifiers like generator
    }
}