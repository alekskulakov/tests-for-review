using System;
using System.IO;
using NUnit.Framework;
using TestsForReview.Utils;
using TestsForReview.WebDriverExt;
namespace TestsForReview.Tests
{
    [SetUpFixture]
    public class TestsSetUp
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.ScreenShotPath);
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
            Directory.CreateDirectory(dir);
        }
        
        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            DriversController.Instance.RemoveAll();
        }
    }
}
