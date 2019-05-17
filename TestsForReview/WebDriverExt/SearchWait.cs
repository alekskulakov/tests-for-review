using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestsForReview.WebDriverExt
{
    public class SearchWait : DefaultWait<ISearchContext>
    {
        private static TimeSpan DefaultSleepTimeout => TimeSpan.FromMilliseconds(500.0);

        public SearchWait(ISearchContext searchContext, TimeSpan timeout)
            : this(new SystemClock(), searchContext, timeout, DefaultSleepTimeout)
        {
        }

        public SearchWait(IClock clock, ISearchContext searchContext, TimeSpan timeout, TimeSpan sleepInterval)
            : base(searchContext, clock)
        {
            Timeout = timeout;
            PollingInterval = sleepInterval;
            IgnoreExceptionTypes(typeof(NotFoundException));
        }
    }

    public class BaseElementWait : DefaultWait<BasePageElement>
    {
        private static TimeSpan DefaultSleepTimeout => TimeSpan.FromMilliseconds(500.0);

        public BaseElementWait(BasePageElement pageElement, TimeSpan timeout)
            : this(new SystemClock(), pageElement, timeout, DefaultSleepTimeout)
        {
        }

        public BaseElementWait(IClock clock, BasePageElement pageElement, TimeSpan timeout, TimeSpan sleepInterval)
            : base(pageElement, clock)
        {
            Timeout = timeout;
            PollingInterval = sleepInterval;
            IgnoreExceptionTypes(typeof(NotFoundException));
        }
    }
}
