using System.Drawing;
using Size = TestsForReview.Pages.Size;

namespace TestsForReview.Models
{
    public class ProductItem
    {
        public string Name { private set; get; }

        public static ProductItem GetDefault()
        {
            return new ProductItem()
            {
                Name = "Hero Hoodie"
            };
        }

        public ProductOrder ToOrder(Size? size, Color color)
        {
            return new ProductOrder(this){Size = size, Color = color};
        }
    }

    public class ProductOrder
    {
        public ProductItem Product;
        public Size? Size;
        public Color Color;

        public ProductOrder(ProductItem product)
        {
            Product = product;
        }
    }
}
