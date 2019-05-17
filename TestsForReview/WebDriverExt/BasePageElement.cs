using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TestsForReview.Interfaces;
using TestsForReview.Utils;

namespace TestsForReview.WebDriverExt
{
    public class BasePageElement: IWrapsElement, IEquatable<string>, IAssert<string>, IHasLocator
    {
        public ILocator Locator { get; set; }

        public IWebElement WrappedElement => Locator.LocateElement();

        internal IWebDriver Executor => DriversController.Instance.GetWrapped().Driver;

        public BasePageElement(ILocator elementLocator)
        {
            Locator = elementLocator;
        }
        
        public BasePageElement(By by)
        {
            Locator = LocatorFactory.Create(by);
        }
        
        public BasePageElement()
        {
        }

        public virtual bool Invisible
        {
            get
            {
                try
                {
                    var element = WrappedElement;
                    if (element == null)
                        return true;
                    return !Helper.TryGet(() => element.Displayed);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        /*
        public T CreateChild<T>(By by) where T : BasePageElement, new()
        {
            if (by == null)
            {
                throw new ArgumentNullException(nameof(by), "List of criteria may not be null");
            }

            var element = new T {Locator = BuildLocator(by)};
            element.Initialize();
            return element;
        }
        */
        public T CreateChild<T>() where T : BasePageElement, new()
        {
            var element = new T();
            element.Locator.SetParentLocator(Locator);
            element.Initialize();
            return element;
        }
        /*
        internal ILocator BuildLocator(By by)
        {
            var locator = Locator != null ? LocatorFactory.Create(Locator, by) : LocatorFactory.Create(by);
            return locator;
        }
        */
        public virtual void Initialize()
        {
        }

        public virtual void Click()
        {
            ScrollTo();
            Helper.TryGet(() =>
            {
                var element = WrappedElement;
                element.Click();
                return element;
            });
        }

        public void ScrollTo()
        {
            WaitForElementClickableWd();

            Func<IWebElement> scrollFunc = () =>
            {
                var actions = new OpenQA.Selenium.Interactions.Actions(Executor);
                actions.MoveToElement(WrappedElement);
                actions.Perform();
                return WrappedElement;
            };

            Helper.TryGet(scrollFunc);
        }

        public void Focus()
        {
            ScrollTo();
            Helper.TryGet(() =>
            {
                new OpenQA.Selenium.Interactions.Actions(Executor).MoveToElement(WrappedElement).Perform();
                return true;
            });
        }

        internal void WaitForElementClickableWd()
        {
            IWebElement ScrollFunc
            () => new WebDriverWait(Executor, Config.ElementLoadTimeoutTimeSpan).Until(driver => ExpectedConditions.ElementToBeClickable(WrappedElement).Invoke(driver));

            Helper.TryGet(ScrollFunc);
        }

        public string GetCssValue(string propertyName)
        {
            return Helper.TryGet(() => WrappedElement.GetCssValue(propertyName));
        }

        public string GetAttribute(string attributeName)
        {
            return Helper.TryGet(() => WrappedElement.GetAttribute(attributeName));
        }

        public string[] SplitClassAttr()
        {
            return GetAttribute("class").Split(' ');
        }

        public void WaitFor(Func<BasePageElement, bool> func)
        {
            BasePageElement WaitForFunc() =>
                new WebDriverWait(Executor, Config.ElementLoadTimeoutTimeSpan).Until(driver =>
                    func.Invoke(this) ? this : null);

            Helper.TryGet(WaitForFunc);
        }

        public void WaitFor(Func<bool> func, string exceptionMessage = "")
        {
            Func<BasePageElement> waitForFunc = () =>
                new WebDriverWait(Executor, Config.ElementLoadTimeoutTimeSpan)
                    .Until(driver => func.Invoke() ? this : null);

            Helper.TryGet(waitForFunc);
        }

        public bool IsExist()
        {
            return Locator.LocateElements().Count != 0;
        }

        public virtual string Text
        {
            get { return Helper.TryGet(() =>
                {
                    var innerElement = WrappedElement is IWrapsElement element
                        ? element.WrappedElement
                        : WrappedElement;

                    if (innerElement is RemoteWebElement remoteElement)
                    {
                        var location = remoteElement.LocationOnScreenOnceScrolledIntoView;
                    }
                    
                    return WrappedElement.Text;
                });
            }
        }

        public virtual bool Displayed
        {
            get { return Helper.TryGet(() => WrappedElement.Displayed); }
        }

        public virtual bool Enabled
        {
            get { return Helper.TryGet(() => WrappedElement.Enabled); }
        }

        public virtual bool Selected
        {
            get { return Helper.TryGet(() => WrappedElement.Selected); }
        }

        public void WaitForElementVisible()
        {
            var wait = new BaseElementWait(this, Config.ElementLoadTimeoutTimeSpan);
            wait.Until(element => !element.Invisible);
        }

        public bool WaitForElementInvisible()
        {
            var wait = new BaseElementWait(this, Config.ElementLoadTimeoutTimeSpan);
            return wait.Until(element => element.Invisible);
        }

        public void SendKeys(string value)
        {
            Helper.TryGet(() =>
            {
                WrappedElement.SendKeys(value);
                return value;
            });
        }

        public virtual void Clear()
        {
            Helper.TryGet(() =>
            {
                WrappedElement.Clear();
                return true;
            });
        }

        public T As<T>() where T: BasePageElement, new()
        {
            var newItem = new T { Locator = Locator };
            return newItem;
        }

        public virtual bool Equals(string other)
        {
            return other != null && other.Equals(Text);
        }

        public virtual void AssertIsEqual(string expected)
        {
            Assert.AreEqual(expected, Text);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
