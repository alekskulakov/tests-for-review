using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Pages
{
    public class MessageListObj : BasePageElement
    {
        [FindsBy(How = How.CssSelector, Using = ".message")]
        public ElementListProxy<BasePageElement> Messages;

        public MessageListObj() : base(By.CssSelector(".page.messages")) { }
    }
}
