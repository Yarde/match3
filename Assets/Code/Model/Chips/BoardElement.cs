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

        private void OnDestroy()
        {
            OnMove = null;
            OnClick = null;
            OnEffect = null;
        }

        public abstract Func<BoardCell, bool> GetEffectPredicate();
        public abstract bool CheckMatch(BoardSettings settings, Vector2Int index, BoardCell[,] boardCells);
    }
}