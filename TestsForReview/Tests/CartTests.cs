using System.Drawing;
using NUnit.Framework;
using TestsForReview.Actions;
using TestsForReview.Models;
using TestsForReview.Utils;
using Size = TestsForReview.Pages.Size;

namespace TestsForReview.Tests
{
    public class CartTests : TestClassBase
    {
        [TearDown]
        public override void Cleanup()
        {
            //Api.RemoveOrders();
            base.Cleanup();
        }

        [Test]
        public void AddToCartFromHomePage_WithOptions()
        {
            var productToAdd = ProductActions.CreateProductWithCategory();

            AccountActions.SignIn(Account.GetDefault());
            ProductActions.AddToCart(productToAdd.ToOrder(Size.M, Color.Black));
            MessageActions.WaitAndCheckMessages($"You added {productToAdd.Name} to your shopping cart.");
        }
    }
}