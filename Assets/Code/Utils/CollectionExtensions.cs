using System;
using System.Collections.Generic;
using Code.Model;

namespace Code.Utils
{
    public static class CollectionExtensions
    {
        public static T Random<T>(this IReadOnlyList<T> items) => Random(items, new Random(Environment.TickCount));

        public static T Random<T>(this IReadOnlyList<T> items, Random rng) =>
            items.Count == 0 ? default : items[rng.Next(items.Count)];
    }
}