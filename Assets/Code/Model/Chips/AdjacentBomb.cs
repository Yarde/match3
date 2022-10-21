using System;
using UnityEngine;

namespace Code.Model.Chips
{
    [CreateAssetMenu]
    public class AdjacentBomb : BoardElement
    {
        public override Func<BoardCell, bool> GetEffectPredicate()
        {
            // destroy adjacent objects
            return null;
        }

        public override bool CheckMatch(BoardSettings settings, Vector2Int vector2Int, BoardCell[,] boardCells)
        {
            // bomb doesn't match, you need to click it
            return false;
        }
    }
}