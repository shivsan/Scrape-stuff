using System;
using System.Collections.Generic;
using Scrape_items_to_buy.Websites.Sites;
using Objects.Websites;
using Objects.Websites.Sites;

namespace c
{
    public class program
    {
        public static void Main(string [] args)
        {
            var websitesToSearch = GetAllWebsites();
            int LowestRetailPrice = int.MaxValue;
            Website lowestRetailWebsite;
            Product lowestRetailProduct;

            foreach(var website in websitesToSearch)
            {
                var productName = args[0];
                website.ProductName = productName;

                //Fails in case the product is not available
                try
                {
                    var product = website.GetProduct();

                    if (product != null && product.Price!=0 && product.Price < LowestRetailPrice)
                    {
                        lowestRetailProduct = product;
                        lowestRetailWebsite = website;
                        LowestRetailPrice = product.Price;
                    }
                }
                finally
                {

                }
            }
        }

        private static List<Website> GetAllWebsites()
        {
            List<Website> websitesToSearch = new List<Website>();
            websitesToSearch.Add(new FlipKart());
            websitesToSearch.Add(new Amazon());
            websitesToSearch.Add(new SnapDeal());
            websitesToSearch.Add(new quikr());
            websitesToSearch.Add(new Olx());
            websitesToSearch.Add(new eBay());
            return websitesToSearch;
        }
    }
}