using System;
using System.Collections.Generic;
using Scrape_items_to_buy.Websites.Sites;
using Objects.Websites;
using Objects.Websites.Sites;
using FileUtils;

namespace c
{
    public class program
    {
        public static void Main(string [] args)
        {
            var listGames = GamesCSV.ReadGames(@"C:\Users\skumarsekar\Documents\Pet Projects\Scrape items to buy\Scrape items to buy\Files\PS4 games.txt");
            var ignoreList = IgnoreList.ReadIgnoreList(@"C:\Users\skumarsekar\Documents\Pet Projects\Scrape items to buy\Scrape items to buy\Files\Ignore List.txt");

            foreach (var listGame in listGames)
            {
                var websitesToSearch = GetAllWebsites();
                int LowestRetailPrice = int.MaxValue;
                Website lowestRetailWebsite;
                Product lowestRetailProduct;
                foreach (var website in websitesToSearch)
                {
                    var productName = listGame.GameName + " PS4";
                    website.ProductName = productName;

                    //Fails in case the product is not available
                    try
                    {
                        var product = website.GetProduct();

                        
                        if (product != null && product.Price != 0 && product.Price < LowestRetailPrice)
                        {
                            if (ignoreList.Exists(i => i.GameURL.Contains(product.Url) || product.Url.Contains(i.GameURL)))
                                continue;

                            lowestRetailProduct = product;
                            lowestRetailWebsite = website;
                            LowestRetailPrice = product.Price;
                        }
                    }
                    finally
                    {
                        if (LowestRetailPrice < listGame.GamePrice)
                        {
                            
                        }
                    }
                }
            }
        }

        private static List<Website> GetAllWebsites()
        {
            List<Website> websitesToSearch = new List<Website>();
            //websitesToSearch.Add(new FlipKart());
            //websitesToSearch.Add(new Amazon());
            //websitesToSearch.Add(new SnapDeal());            
            websitesToSearch.Add(new Olx());
            websitesToSearch.Add(new eBay());
            return websitesToSearch;
        }
    }
}