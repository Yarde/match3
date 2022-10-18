using System;
using Code.View;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Model.Chips
{
    public abstract class BoardElement : ScriptableObject
    {
        public Sprite sprite;
        public ChipView prefab;
        public bool isSwappable;
        public bool isClickable;

        public Func<Vector2Int, Vector2Int, UniTask> OnMove;
        public Func<UniTask> OnClick;
        public Func<UniTask> OnEffect;

        public abstract void ApplyEffect();
    }
}