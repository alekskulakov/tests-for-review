using TestsForReview.Models;
using TestsForReview.Pages;
using TestsForReview.Utils;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Actions
{
    public static class ProductActions
    {
        public static ProductItem CreateProductWithCategory()
        {
            return ProductItem.GetDefault();
        }

        public static void AddToCart(ProductOrder order)
        {
            var product = PageObjects.Get<ProductListObj>().Products.GetItem(order.Product);
            product.Focus();
            product.SizeItems.GetItem(order.Size.ToString()).Click();
            product.ColorItems.GetItem(order.Color).Click();
            product.AddToCart.Click();

            AccountActions.WaitLoggedIn();
        }
    }
}
