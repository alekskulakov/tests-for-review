using System;
using OpenQA.Selenium;
using TestsForReview.Interfaces;
using TestsForReview.Pages;

namespace TestsForReview.WebDriverExt
{
    public sealed class LocatorFactory
    {
        private static Func<bool> WaitTo => () =>
        {
            var body = PageObjects.Get<BodyObj>();
            body.WaitFor(element => element.IsExist());
            return body.IsLoaded();
        };

        public static ILocator Create(By by)
        {
            return new CustomElementLocator(by, WaitTo);
        }

        public static ILocator Create(ILocator parentLocator, By by)
        {
            return new CustomElementLocator(parentLocator, by, WaitTo);
        }
    }
}
