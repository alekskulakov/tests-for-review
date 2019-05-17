using System;

namespace TestsForReview.Utils
{
    [Serializable]
    public sealed class TestingFrameworkException : Exception
    {
        public TestingFrameworkException() : base()
        {
        }

        public TestingFrameworkException(string message) : base(message)
        {
        }

        public TestingFrameworkException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
