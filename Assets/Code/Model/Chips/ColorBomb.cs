using System;
using UnityEngine;

namespace Code.Model.Chips
{
    [CreateAssetMenu]
    public class ColorBomb : SimpleColorChip
    {
        public override Func<BoardCell, BoardCell, bool> GetEffectPredicate()
        {
            bool Predicate(BoardCell source, BoardCell target)
            {
                return target.chip is SimpleColorChip chip && chip.color.name == color.name;
            }

            return Predicate;
        }
    }
}