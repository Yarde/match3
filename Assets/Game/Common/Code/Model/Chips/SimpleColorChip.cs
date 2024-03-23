using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Code.Model.Chips
{
    [CreateAssetMenu]
    public class SimpleColorChip : BoardElement
    {
        public Color color;
        public ParticleSystem particleSystem;

        public override Func<BoardCell, BoardCell, bool> GetEffectPredicate()
        {
            return null;
        }

        public override bool CheckMatch(BoardSettings settings, Vector2Int index, BoardCell[,] boardCells)
        {
            return HorizontalMatchCheck(settings, index, boardCells) || VerticalMatchCheck(settings, index, boardCells);
        }

        private bool VerticalMatchCheck(BoardSettings settings, Vector2Int index, BoardCell[,] boardCells)
        {
            var count = 0;
            for (var j = index.y - 1; j >= 0; j--)
            {
                if (!CompareColors(boardCells[index.x, j], color)) break;
                count++;
            }

            for (var j = index.y + 1; j < settings.boardSize.y; j++)
            {
                if (!CompareColors(boardCells[index.x, j], color)) break;
                count++;
            }

            return count > 1;
        }

        private bool HorizontalMatchCheck(BoardSettings settings, Vector2Int index, BoardCell[,] boardCells)
        {
            var count = 0;
            for (var i = index.x - 1; i >= 0; i--)
            {
                if (!CompareColors(boardCells[i, index.y], color)) break;
                count++;
            }

            for (var i = index.x + 1; i < settings.boardSize.x; i++)
            {
                if (!CompareColors(boardCells[i, index.y], color)) break;
                count++;
            }

            return count > 1;
        }

        private static bool CompareColors(BoardCell target, Object sourceColor)
        {
            var chip = target.chip as SimpleColorChip;
            if (chip && chip.color)
            {
                return chip.color.name == sourceColor.name;
            }

            return false;
        }
    }
}