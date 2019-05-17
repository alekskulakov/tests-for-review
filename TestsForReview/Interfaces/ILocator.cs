using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace TestsForReview.Interfaces
{
    public interface ILocator : ICloneable
    {
        By By { get; }
        ILocator ParentLocator { get; }
        Func<bool> WaitFunc { get; set; }
        IWebElement LocateElement(bool useCache = true);
        ReadOnlyCollection<IWebElement> LocateElements();
        ISearchContext SearchContext { get; }
        int? Index { get; set; }
        void SetParentLocator(ILocator parentLocator);
        void CopyFrom(ILocator fromLocator);
    }
}
