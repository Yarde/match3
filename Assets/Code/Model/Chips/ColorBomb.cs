using System;
using UnityEngine;

namespace Code.Model.Chips
{
    [CreateAssetMenu]
    public class ColorBomb : SimpleColorChip
    {
        public override Func<BoardCell, bool> GetEffectPredicate()
        {
            bool Predicate(BoardCell cell)
            {
                return cell.chip is SimpleColorChip chip && chip.color.name == color.name;
            }

            return Predicate;
        }
    }
}