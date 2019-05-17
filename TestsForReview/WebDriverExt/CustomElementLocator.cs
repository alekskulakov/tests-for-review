using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using TestsForReview.Interfaces;
using TestsForReview.Utils;

namespace TestsForReview.WebDriverExt
{
    public class CustomElementLocator : ILocator
    {
        public Func<bool> WaitFunc { get; set; }

        public ILocator ParentLocator { get; set; }
        public By By { get; private set; }

        public int? Index { get; set; }

        public CustomElementLocator(By by, Func<bool> waitFunc)
        {
            By = by;
            WaitFunc = waitFunc;
        }

        public CustomElementLocator(ILocator parentLocator, By by, Func<bool> waitFunc) : this(by, waitFunc)
        {
            ParentLocator = parentLocator;
        }

        public ISearchContext SearchContext
        {
            get
            {
                if (ParentLocator != null)
                    return ParentLocator.LocateElement();

                return DriversController.Instance.GetWrapped().Driver;
            }
        }

        public IWebElement LocateElement(bool userCache)
        {
            var elements = LocateElements().ToArray();

            if (elements.Length == 0)
                throw new NoSuchElementException($"Could not find element by: {By}");

            if (!Index.HasValue && elements.Length != 1)
                throw new TestingFrameworkException($"More than one element by: {By}");

            if (Index.HasValue && Index >= elements.Length)
                throw new TestingFrameworkException();

            return elements[Index ?? 0];
        }

        public ReadOnlyCollection<IWebElement> LocateElements()
        {
            WaitFunc?.Invoke();

            Func<ReadOnlyCollection<IWebElement>> getElementsFunc = () => SearchContext.FindElements(By);
            return Helper.TryGet(getElementsFunc);
        }

        public object Clone()
        {
            return new CustomElementLocator(ParentLocator, By, WaitFunc);
        }

        public void SetParentLocator(ILocator parentLocator)
        {
            ParentLocator = parentLocator;
        }

        public void CopyFrom(ILocator fromLocator)
        {
            By = fromLocator.By;
            ParentLocator = fromLocator.ParentLocator;
            WaitFunc = fromLocator.WaitFunc;
        }
    }
}
