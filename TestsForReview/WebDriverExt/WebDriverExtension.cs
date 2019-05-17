using System;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using TestsForReview.Utils;

namespace TestsForReview.WebDriverExt
{
    public static class WebDriverExtension
    {
        public static void NoCacheRefresh(this IWebDriver webDriver)
        {
            var actionObject = new OpenQA.Selenium.Interactions.Actions(webDriver);
            actionObject.KeyDown(Keys.Control).SendKeys(Keys.F5).KeyUp(Keys.Control).Perform();
        }

        public static void AddCookie(this IWebDriver webDriver, string cookieName, string cookieValue)
        {
            webDriver.Manage().Cookies.AddCookie(new Cookie(cookieName, cookieValue));
        }

        public static void GoToUrl(this IWebDriver webDriver, string url)
        {
            webDriver.Navigate().GoToUrl(url);
        }

        public static void DeleteCookieNamed(this IWebDriver webDriver, string cookieName)
        {
            webDriver.Manage().Cookies.DeleteCookieNamed(cookieName);
        }

        public static void Refresh(this IWebDriver webDriver)
        {
            webDriver.Navigate().Refresh();
            //webDriver.WaitForSiteLoad();
        }

        public static void SaveTestOutputs(this IWebDriver webDriver, string testName)
        {
            try
            {
                testName = Helper.RemoveInvalidChars(testName);

                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.ScreenShotPath, testName);

                if (dir.Length > 240)
                    dir = dir.Substring(0, 240);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                else
                {
                    Directory.Delete(dir, true);
                }
                var filePath = Path.Combine(dir, "page.html");
                File.WriteAllText(filePath, webDriver.PageSource);

                filePath = Path.Combine(dir, "page.png");

                var screenshot = webDriver.TakeScreenshot();
                screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);

                var logs = webDriver.Manage().Logs.GetLog("browser");
                if (logs == null || logs.Count <= 0) return;

                var textLog = Enumerable.Aggregate<string>(logs.Select(l => l.ToString()), (workingSentence, next) => workingSentence + "\r\n" + next);
                filePath = Path.Combine(dir, "console.txt");
                File.WriteAllText(filePath, textLog);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured when saving test outputs of failed test for test {testName}");
                Console.WriteLine(e.Message);
            }
        }
/*
        public static void WaitForSiteLoad(this IWebDriver driver)
        {
            new WebDriverWait(driver, Config.ElementLoadTimeoutTimeSpan).Until(d => d.Title.Equals(StringResources.SiteTitle));
        }
        */
        public static void WriteToConsole(this IWebDriver driver, string message)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript($"console.log('{message}')");
        }

        public static void Start(this IWebDriver driver)
        {
 //           driver.Navigate().GoToUrl($"{Config.StartPageUrl}api/test/AuthUser?sid={user.SID}");
//            driver.Navigate().GoToUrl(Config.HomePageUrl);
        }
    }
}
