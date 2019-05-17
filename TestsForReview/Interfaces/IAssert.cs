namespace TestsForReview.Interfaces
{
    public interface IAssert<in T>
    {
        void AssertIsEqual(T expected);
    }
}
