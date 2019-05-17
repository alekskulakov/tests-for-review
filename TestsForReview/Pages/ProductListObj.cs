using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestsForReview.Models;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Pages
{
    public enum Size
    {
        XS,
        S,
        M,
        L,
        XL
    }

    public class ColorObj : BasePageElement, IEquatable<Color>
    {
        public string ColorName => GetAttribute("option-label");

        public bool Equals(Color other)
        {
            return ColorName.Equals(other.Name);
        }
    }

    public class ProductItemObj : BasePageElement, IEquatable<ProductItem>
    {
        [FindsBy(How = How.ClassName, Using = "product-item-name")]
        public BasePageElement Name;

        [FindsBy(How = How.CssSelector, Using = ".swatch-attribute.size .swatch-option")]
        public ElementListProxy<BasePageElement> SizeItems;

        [FindsBy(How = How.CssSelector, Using = ".swatch-attribute.color .swatch-option")]
        public ElementListProxy<ColorObj> ColorItems;

        [FindsBy(How = How.CssSelector, Using = ".action.tocart.primary")]
        public BasePageElement AddToCart;

        public bool Equals(ProductItem other)
        {
            return Name.Equals(other.Name);
        }
    }

    public class ProductListObj : BasePageElement
    {
        [FindsBy(How = How.ClassName, Using = "product-item")]
        public ElementListProxy<ProductItemObj> Products;

        public ProductListObj() : base(By.ClassName("product-items")) { }
    }
}
