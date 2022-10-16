using System;
using System.Collections.Generic;

namespace Code.Utils
{
    public static class CollectionExtensions
    {
        public static T Random<T>(this IReadOnlyList<T> items, Random rng) =>
            items.Count == 0 ? default : items[rng.Next(items.Count)];
    }
}