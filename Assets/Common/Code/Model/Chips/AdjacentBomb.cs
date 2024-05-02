using System;
using Common.Code.Utils;
using UnityEngine;

namespace Common.Code.Model.Chips
{
    [CreateAssetMenu]
    public class AdjacentBomb : BoardElement
    {
        public override Func<BoardCell, BoardCell, bool> GetEffectPredicate()
        {
            bool Predicate(BoardCell source, BoardCell target)
            {
                var hasChip = target.chip != null;
                return hasChip && target.Index.IsAdjacent(source.Index);
            }

            return Predicate;
        }

        public override bool CheckMatch(BoardSettings settings, Vector2Int vector2Int, BoardCell[,] boardCells)
        {
            // bomb doesn't match, you need to click it
            return false;
        }
    }
}