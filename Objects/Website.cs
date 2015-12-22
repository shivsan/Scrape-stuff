using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileUtils;

namespace Objects.Websites.Sites
{
    public abstract class Website : IWebSite
    {
        public abstract Product GetProduct();
        public abstract string ParseProductName(string productName);
        public abstract string SearchURL(string productName);
        public string ProductName;

        public bool BlackListDescription(string body)
        {
            var listGames = BlackListList.ReadBlackListList(@"C:\Users\skumarsekar\Documents\Pet Projects\Scrape items to buy\Scrape items to buy\Files\Blacklist descriptions.txt");

            return listGames.Any(l => body.ToLower().Contains(l.BlackListText.ToLower()));
        }

        public virtual double GetPrice(double price, string webSite)
        {
            var DiscountsList = DiscountList.ReadDiscountList(@"C:\Users\skumarsekar\Documents\Pet Projects\Scrape items to buy\Scrape items to buy\Files\Discounts.txt");

            foreach (var discount in DiscountsList)
            {
                if (discount.Website.ToLower().Equals(webSite))
                {
                    price = price*(100 - discount.DiscountPercent)/100;
                    price = price - discount.DiscountAmount;
                }
            }

            return price;
        }
    }
}
