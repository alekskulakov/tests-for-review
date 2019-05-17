using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Pages.AdminPages
{
    public class LoginModalObj : BasePageElement
    {
        [FindsBy(How = How.CssSelector, Using = "*[name='login[username]']")]
        public BasePageElement UserName;

        [FindsBy(How = How.CssSelector, Using = "*[name='login[password]']")]
        public BasePageElement Password;

        [FindsBy(How = How.CssSelector, Using = ".action-login.action-primary")]
        public BasePageElement SignIn;

        public LoginModalObj() : base(By.Id("login-form")) { }
    }
}
