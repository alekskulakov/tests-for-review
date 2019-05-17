using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestsForReview.Interfaces;

namespace TestsForReview.WebDriverExt
{
    public class ElementListProxy<T> : IHasLocator, IEnumerable<T> where T : BasePageElement, new()
    {
        public ILocator Locator { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return Locator.LocateElements().Select((e, i) =>
            {
                var indexedItem = new T {Locator = (ILocator) Locator.Clone()};
                indexedItem.Locator.Index = i;
                CustomPageFactory.InitElements(indexedItem);

                indexedItem.Initialize();
                return indexedItem;
            }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void SetNoWait()
        {
            Locator.WaitFunc = null;
        }
    }
}
