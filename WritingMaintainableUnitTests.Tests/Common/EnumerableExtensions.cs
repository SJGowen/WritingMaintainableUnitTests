using System;
using System.Collections.Generic;

namespace WritingMaintainableUnitTests.Tests.Common
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            var index = 0;
            foreach(var item in items)
            {
                action(item, index);
                index += 1;
            }
        }
    }
}