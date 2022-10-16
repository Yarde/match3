using System;
using System.Collections.Generic;
using Code.Model.Chips;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Model
{
    [CreateAssetMenu]
    public class BoardSettings : ScriptableObject
    {
        public Vector2Int boardSize;
        public List<BoardCell> initialLayout;

        [HideInInspector] public bool IsValid;

        private void OnValidate()
        {
            IsValid = initialLayout.Count / boardSize.x == boardSize.y;
        }
    }

    [Serializable]
    public class BoardCell
    {
        public BoardElement chip;
        // optional extra modifiers like generator
    }
}