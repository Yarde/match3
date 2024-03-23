using System;
using System.Collections.Generic;
using Common.Code.Model.Chips;
using UnityEngine;

namespace Common.Code.Model
{
    [CreateAssetMenu]
    public class BoardSettings : ScriptableObject
    {
        public Vector2Int boardSize;
        public List<BoardElement> initialLayout;
        public BoardVisuals boardVisuals;

        [HideInInspector] public bool isValid;
        public bool allowNonMatchSwipe = true;

        private void OnValidate()
        {
            isValid = initialLayout.Count / boardSize.x == boardSize.y;
        }
    }

    [Serializable]
    public class BoardVisuals
    {
        public Sprite background;
        public Sprite boardFrame;

        public Sprite boardBackground;

        public float cellSize = 40f;
        public float boardOffset = 20f;
    }

    [Serializable]
    public class BoardCell
    {
        public Vector2Int Index { get; set; }
        public BoardElement chip;
        public BoardElement obstacle;

        public bool IsBlocked => obstacle;
        // optional extra modifiers like generator, ice...

        public bool CheckMatch(BoardSettings settings, BoardCell[,] boardCells)
        {
            return !IsBlocked && chip != null && chip.CheckMatch(settings, Index, boardCells);
        }
    }
}