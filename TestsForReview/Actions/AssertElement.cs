using NUnit.Framework;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Actions
{
    public static class AssertElement
    {
        public static void AreEqual(BasePageElement actual, string expected)
        {
            if (string.IsNullOrEmpty(expected))
            {
                Assert.That(actual.Text, Is.EqualTo(string.Empty));
            }
            else
            {
                Assert.That(actual.IsExist());
                Assert.That(actual.Text, Is.EqualTo(expected));
            }
        }
    }
}
