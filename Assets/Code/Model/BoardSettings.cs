﻿using System;
using System.Collections.Generic;
using Code.Model.Chips;
using UnityEngine;

namespace Code.Model
{
    [CreateAssetMenu]
    public class BoardSettings : ScriptableObject
    {
        public Vector2Int boardSize;
        public List<BoardElement> initialLayout;
        public BoardVisuals boardVisuals;

        [HideInInspector] public bool IsValid;
        public bool allowNonMatchSwipe = true;

        private void OnValidate()
        {
            IsValid = initialLayout.Count / boardSize.x == boardSize.y;
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
        [HideInInspector] public Vector2Int index;
        public BoardElement chip;
        // optional extra modifiers like generator
    }
}