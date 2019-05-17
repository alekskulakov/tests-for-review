using System;

namespace TestsForReview.WebDriverExt
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class RootByAttribute : Attribute
    { }
}
