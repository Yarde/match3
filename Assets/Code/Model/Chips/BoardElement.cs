using System;
using Code.View;
using UnityEngine;

namespace Code.Model.Chips
{
    public abstract class BoardElement : ScriptableObject
    {
        public Sprite sprite;
        public ChipView prefab;
        public bool isSwappable;
        public bool isClickable;

        public Action<Vector2Int> OnSwap;
        public Action OnClick;
        public Action OnEffect;

        public abstract void ApplyEffect();
    }
}