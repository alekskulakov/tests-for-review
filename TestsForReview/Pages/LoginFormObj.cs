using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Pages
{
    public class LoginFormObj : BasePageElement
    {
        [FindsBy(How = How.CssSelector, Using = "*[name='login[username]']")]
        public BasePageElement UserName;

        [FindsBy(How = How.CssSelector, Using = "*[name='login[password]']")]
        public BasePageElement Password;

        [FindsBy(How = How.CssSelector, Using = ".action.login.primary")]
        public BasePageElement SignIn;

        public LoginFormObj() : base(By.CssSelector(".login-container")) { }
    }
}
