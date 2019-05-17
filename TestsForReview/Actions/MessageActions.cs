using System.Linq;
using TestsForReview.Pages;
using TestsForReview.Utils;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Actions
{
    public static class MessageActions
    {
        public static void WaitAndCheckMessages(params string[] expectedMessages)
        {
            var actualMessageList = PageObjects.Get<MessageListObj>();
            Helper.TryGet(() => actualMessageList.Messages.Count().Equals(expectedMessages.Length));

            AssertCollections.AreEqual(actualMessageList.Messages, expectedMessages);
        }


    }
}
