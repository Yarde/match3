using UnityEngine;

namespace Common.Code.Utils
{
    public static class VectorExtensions
    {
        public static bool InBounds2D(this Vector2Int dimensions, Vector2Int position)
        {
            return position.x < 0 || position.x >= dimensions.x || position.y < 0 || position.y >= dimensions.y;
        }
        
        public static bool IsAdjacent(this Vector2Int target, Vector2Int source)
        {
            var distanceX = Mathf.Abs(target.x - source.x);
            var distanceY = Mathf.Abs(target.y - source.y);
            return distanceX <= 1 && distanceY <= 1;
        }
    }
}