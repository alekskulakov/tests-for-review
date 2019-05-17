using OpenQA.Selenium;
using TestsForReview.Interfaces;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Pages
{
    public class BodyObj : BasePageElement, ISingleInstance
    {
        public bool IsLoaded()
        {
            var ariaBusy = GetAttribute("aria-busy");
            return !string.IsNullOrEmpty(ariaBusy) && ariaBusy.Equals("false");
        }

        public BodyObj() : base(By.TagName("body"))
        {
            Locator.WaitFunc = null;
        }
    }
}
