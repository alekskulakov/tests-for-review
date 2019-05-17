using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestsForReview.Interfaces;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Pages
{
    public class TopMenuObj : BasePageElement, ISingleInstance
    {
        [FindsBy(How = How.CssSelector, Using = ".authorization-link>a")]
        public BasePageElement SignIn;

        [FindsBy(How = How.CssSelector, Using = ".customer-name")]
        public BasePageElement CustomerName;

        [FindsBy(How = How.CssSelector, Using = ".greet.welcome span:nth-child(1)")]
        public BasePageElement WelcomeMsg;

        public TopMenuObj() : base(By.CssSelector(".panel.header")) { }
    }
}
