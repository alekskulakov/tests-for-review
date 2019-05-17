using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestsForReview.Utils;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Actions
{
    public static class AssertCollections
    {
        public static void AreEqual<TActual, TExpected>(
            IEnumerable<TActual> actualItems,
            TExpected[] expectedItems, params Action<TActual, TExpected>[] validations) where TActual : class
        {
            var actualItemsArray = actualItems.ToArray();
            expectedItems = expectedItems.Where(exp => exp != null).ToArray();

            Assert.That(actualItemsArray.Length, Is.EqualTo(expectedItems.Length));

            for (var i = 0; i < expectedItems.Length; i++)
                foreach (var validation in validations)
                {
                    validation.Invoke(actualItemsArray[i], expectedItems[i]);
                }
        }

        public static IEnumerable<TActual> AreEquivalent<TActual, TExpected>(
            IEnumerable<TActual> actualItems,
            TExpected[] expectedItems, params Action<TActual, TExpected>[] validations) where TActual : class, IEquatable<TExpected>
        {
            actualItems = actualItems.ToArray();
            expectedItems = expectedItems.Where(exp => exp != null).ToArray();

            Assert.That(actualItems.Count(), Is.EqualTo(expectedItems.Length));
            foreach (var expectedItem in expectedItems)
            {
                var actualItem = actualItems.GetItem(expectedItem);

                foreach (var validation in validations)
                {
                    validation.Invoke(actualItem, expectedItem);
                }
            }

            return actualItems;
        }

        public static void AreEqual(IEnumerable<BasePageElement> actual, params string[] expected)
        {
            AssertCollections.AreEqual(actual, expected, AssertElement.AreEqual);
        }
    }
}
