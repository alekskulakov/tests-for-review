using NUnit.Framework;
using TestsForReview.Actions;
using TestsForReview.Models;
using TestsForReview.Pages;
using TestsForReview.Pages.AdminPages;
using TestsForReview.Utils;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Tests
{
    public class SignInTests : TestClassBase
    {
        [Test]
        public void WelcomeMessageContainsFullUserName()
        {
            var account = Account.GetDefault();

            AccountActions.SignIn(account);

            AssertElement.AreEqual(PageObjects.Get<TopMenuObj>().WelcomeMsg, $"Welcome, {account.DisplayName}!");
            //check something else
        }

        [Test]
        public void SignIn_WrongPassword()
        {
            AccountActions.TrySignIn(Account.GetDefault().UserName, "123");
            MessageActions.WaitAndCheckMessages("The account sign-in was incorrect or your account is disabled temporarily. Please wait and try again later.");
            AssertElement.AreEqual(PageObjects.Get<TopMenuObj>().WelcomeMsg, "Default welcome msg!");
            //AssertElement.AreEqual(PageObjects.Get<AdminUserObj>().Name, Config.AdminAccount.UserName);
            //check something else
        }

        [Test]
        public void SignInToAdminPage_Valid()
        {
            AccountActions.SignInAsAdmin();
            
            AssertElement.AreEqual(PageObjects.Get<AdminUserObj>().Name, Config.AdminAccount.UserName);
            //check something else
        }
    }
}