using TestsForReview.Models;
using TestsForReview.Pages;
using TestsForReview.Pages.AdminPages;
using TestsForReview.Utils;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Actions
{
    public static class AccountActions
    {
        public static void SignIn(Account acc)
        {
            TrySignIn(acc.UserName, acc.Password);
            
            WaitLoggedIn();
        }

        public static void TrySignIn(string userName, string password)
        {
            NavigationActions.OpenHomePage();

            var topMenu = PageObjects.Get<TopMenuObj>();
            topMenu.SignIn.Click();

            var loginForm = PageObjects.Get<LoginFormObj>();
            loginForm.UserName.SendKeys(userName);
            loginForm.Password.SendKeys(password);
            loginForm.SignIn.Click();
        }

        public static void WaitLoggedIn()
        {
            PageObjects.Get<TopMenuObj>().CustomerName.WaitForElementVisible();

            PageObjects.Get<TopMenuObj>().WelcomeMsg
                .WaitFor(e =>
                    !e.GetAttribute("class").Equals("not-logged-in")
                    && e.GetAttribute("class").Equals("logged-in"));
        }

        public static void SignInAsAdmin()
        {
            SignInAsAdmin(Config.AdminAccount);
        }

        public static void SignInAsAdmin(Account acc)
        {
            NavigationActions.OpenAdminPage();

            var loginForm = PageObjects.Get<LoginModalObj>();
            loginForm.UserName.SendKeys(acc.UserName);
            loginForm.Password.SendKeys(acc.Password);
            loginForm.SignIn.Click();

            loginForm.WaitForElementInvisible();

            PageObjects.Get<AdminUserObj>().Name.WaitForElementVisible();
        }
    }
}
