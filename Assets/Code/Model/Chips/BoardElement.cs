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

        public Func<Move, UniTask> OnMove;
        public Func<UniTask> OnEffect;

        private void OnDestroy()
        {
            OnMove = null;
            OnEffect = null;
        }

        public abstract Func<BoardCell, BoardCell, bool> GetEffectPredicate();
        public abstract bool CheckMatch(BoardSettings settings, Vector2Int index, BoardCell[,] boardCells);
    }

    public struct Move
    {
        public readonly Vector2Int source;
        public readonly Vector2Int target;
        public readonly float delay;

        public Move(Vector2Int source, Vector2Int target, float delay)
        {
            this.source = source;
            this.target = target;
            this.delay = delay;
        }
    }
}