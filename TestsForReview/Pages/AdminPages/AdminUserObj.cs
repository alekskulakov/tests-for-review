using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Pages.AdminPages
{
    public class AdminUserObj :BasePageElement
    {
        [FindsBy(How = How.ClassName, Using = "admin-user-account-text")]
        public BasePageElement Name;

        public AdminUserObj() : base(By.ClassName("admin-user")) { }
    }
}
