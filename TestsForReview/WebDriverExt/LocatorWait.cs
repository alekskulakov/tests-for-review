using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace TestsForReview.WebDriverExt
{
    public class LocatorWait : DefaultWait<IElementLocator>
    {
        private static TimeSpan DefaultSleepTimeout => TimeSpan.FromMilliseconds(500.0);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OpenQA.Selenium.Support.UI.WebDriverWait" /> class.
        /// </summary>
        /// <param name="driver">The WebDriver instance used to wait.</param>
        /// <param name="timeout">The timeout value indicating how long to wait for the condition.</param>
        public LocatorWait(IElementLocator driver, TimeSpan timeout)
            : this(new SystemClock(), driver, timeout, DefaultSleepTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OpenQA.Selenium.Support.UI.WebDriverWait" /> class.
        /// </summary>
        /// <param name="clock">An object implementing the <see cref="T:OpenQA.Selenium.Support.UI.IClock" /> interface used to determine when time has passed.</param>
        /// <param name="driver">The WebDriver instance used to wait.</param>
        /// <param name="timeout">The timeout value indicating how long to wait for the condition.</param>
        /// <param name="sleepInterval">A <see cref="T:System.TimeSpan" /> value indicating how often to check for the condition to be true.</param>
        public LocatorWait(IClock clock, IElementLocator driver, TimeSpan timeout, TimeSpan sleepInterval)
            : base(driver, clock)
        {
            Timeout = timeout;
            PollingInterval = sleepInterval;
            IgnoreExceptionTypes(typeof(NotFoundException));
        }
    }
}
