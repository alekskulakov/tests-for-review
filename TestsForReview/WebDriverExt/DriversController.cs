using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;
using TestsForReview.Utils;

namespace TestsForReview.WebDriverExt
{
    public sealed class DriversController
    {
        private static volatile DriversController _instance;
        private static readonly object SyncRoot = new object();
        private static readonly object LockObj = new object();
        internal static readonly ChromeOptions DriverOptions = new ChromeOptions();

        private readonly ConcurrentBag<ThreadedDriver> _wrappedDrivers;

        static DriversController()
        {
        }

        private DriversController()
        {
            _wrappedDrivers = new ConcurrentBag<ThreadedDriver>();

            DriverOptions.AddArguments("--disable-extensions", "--window-size=1920,1400");

            if (Config.IsHeadless)
                DriverOptions.AddArgument("--headless");

            DriverOptions.SetLoggingPreference(LogType.Browser, LogLevel.All);
        }

        public static DriversController Instance
        {
            get
            {
                if (_instance == null)
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DriversController();
                    }

                return _instance;
            }
        }

        public void SetCurrent(ThreadedDriver driver)
        {
            var driversInThread = GetForCurrentThread().Where(d => !d.Equals(driver));

            foreach (var threadedDriver in driversInThread)
                threadedDriver.IsCurrent = false;

            driver.IsCurrent = true;
        }
        
        private ThreadedDriver GetCurrent()
        {
            return GetForCurrentThread().SingleOrDefault(d => d.IsCurrent);
        }

        private IEnumerable<ThreadedDriver> GetForCurrentThread()
        {
            lock (LockObj)
            {
                return _wrappedDrivers.Where(d => d.Thread.Equals(Thread.CurrentThread)).ToArray();
            }
        }

        public ThreadedDriver New()
        {
            var threadDriver = new ThreadedDriver();
            lock (LockObj)
            {
                _wrappedDrivers.Add(threadDriver);
            }

            SetCurrent(threadDriver);
            return threadDriver;
        }

        public ThreadedDriver GetWrapped()
        {
            var threadDriver = GetCurrent() ?? New();

            return threadDriver;
        }

        public void RemoveForThread()
        {
            lock (LockObj)
            {
                while(GetForCurrentThread().Any())
                //foreach (var threadedDriver in GetForCurrentThread())
                {
                    if (_wrappedDrivers.TryTake(out var driver))
                    {
                        driver?.Driver?.Quit();
                    }
                }
            }
        }

        public void RemoveAll()
        {
            lock (LockObj)
            {
                foreach (var threadDriver in _wrappedDrivers)
                    threadDriver?.Driver?.Quit();

                _wrappedDrivers.Clear();
            }
        }

        public class ThreadedDriver 
        {
            public ThreadedDriver()
            {
                Thread = Thread.CurrentThread;
            }

            public bool IsCurrent;

            private IWebDriver _driver;

            public IWebDriver Driver
            {
                get
                {
                    if (IsInitialized)
                        return _driver;

                    _driver = CreateDriverInternal();
                    try
                    {
                        WebDriverExtension.Start(_driver);
                    }
                    finally
                    {
                        IsInitialized = true;
                    }

                    return _driver;
                }
            }

            public Thread Thread { get; }

            public bool IsInitialized;

            private IWebDriver CreateDriverInternal()
            {
                IWebDriver driver;
                lock (LockObj)
                {
                    var service = ChromeDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory);
                    service.HideCommandPromptWindow = true;

                    driver = Config.WdHubUrl != null
                        ? new RemoteWebDriver(Config.WdHubUrl, DriverOptions.ToCapabilities())
                        : new ChromeDriver(service, DriverOptions);
                }

                var firingDriver = new EventFiringWebDriver(driver);
                firingDriver.ExceptionThrown +=
                    delegate (object sender, WebDriverExceptionEventArgs args)
                    {
                        LogHelper.Error(args.ThrownException);
                    };

                driver = firingDriver;
                driver.Manage().Window.Maximize();

                return driver;
            }
        }
    }
}