using TestsForReview.Utils;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Actions
{
    public static class NavigationActions
    {
        public static void OpenHomePage()
        {
            DriversController.Instance.GetWrapped().Driver.Navigate().GoToUrl(Config.HomePageUrl);
        }

        public static void OpenAdminPage()
        {
            DriversController.Instance.GetWrapped().Driver.Navigate().GoToUrl(Config.AdminPageUrl);
        }
    }
}
