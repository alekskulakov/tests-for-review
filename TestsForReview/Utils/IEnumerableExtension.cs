using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TestsForReview.Utils
{
    public static class EnumerableExtension
    {
        public static TPageObject[] GetItems<TPageObject, TExpected>(this IEnumerable<TPageObject> items,
            TExpected expected) where TPageObject : IEquatable<TExpected>
        {
            IEnumerable<TPageObject> ItemFunc()
            {
                var filteredItems = items.Where(i => i.Equals(expected)).ToArray();
                return filteredItems.Any() ? filteredItems : null;
            }

            var actualItems = Helper.TryGetItem((Func<IEnumerable<TPageObject>>) ItemFunc, $"Couldn't wait for [{typeof(TExpected).Name}] to appear")
                .ToArray();

            Assert.That(actualItems, Is.Not.Null);

            return actualItems;
        }

        public static TPageObject GetItem<TPageObject, TExpected>(
            this IEnumerable<TPageObject> items,
            TExpected expected) where TPageObject : class, IEquatable<TExpected>
        {
            var actualItems = items.GetItems(expected);


            var actualItem = actualItems.FirstOrDefault();
            Assert.That(actualItem, Is.Not.Null);

            return actualItems.First();
        }

    }
}