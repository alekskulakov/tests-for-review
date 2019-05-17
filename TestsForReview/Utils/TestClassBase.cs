using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Utils
{
    public abstract class TestClassBase
    {
        protected IWebDriver Driver => DriversController.Instance.GetWrapped().Driver;

        [SetUp]
        public void Init()
        {
        }

        [OneTimeSetUp]
        public void FixtureInit()
        {
        }

        [OneTimeTearDown]
        public void FixtureCleanup()
        {}

        [TearDown]
        public virtual void Cleanup()
        {
            if (Driver != null)
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    Driver.SaveTestOutputs(TestContext.CurrentContext.Test.Name);
                }
            }

            DriversController.Instance.RemoveForThread();
        }
    }
}
