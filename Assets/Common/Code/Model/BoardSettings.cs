using System;
using Common.Code.ChipGenerator;
using Common.Code.Model.Chips;
using UnityEngine;

namespace Common.Code.Model
{
    [CreateAssetMenu]
    public class BoardSettings : ScriptableObject
    {
        public Vector2Int boardSize;
        public BoardVisuals boardVisuals;
        public ChipGeneratorBase generatorData;

        public bool allowNonMatchSwipe = true;
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