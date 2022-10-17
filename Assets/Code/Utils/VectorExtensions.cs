using UnityEngine;

namespace Code.Utils
{
    public static class VectorExtensions
    {
        public static bool InBounds2D(this Vector2Int dimensions, Vector2Int position)
        {
            return position.x < 0 || position.x >= dimensions.x || position.y < 0 || position.y >= dimensions.y;
        }
    }
}