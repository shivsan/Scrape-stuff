using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
            var listGames = FileListsCSV.ReadGames(@"C:\Users\skumarsekar\Documents\Pet Projects\Scrape items to buy\Scrape items to buy\Files\PS4 games.txt");
            var ignoreList = IgnoreList.ReadIgnoreList(@"C:\Users\skumarsekar\Documents\Pet Projects\Scrape items to buy\Scrape items to buy\Files\Ignore List.txt");
            
            foreach (var listGame in listGames)
            {
                var websitesToSearch = GetAllWebsites();
                double LowestRetailPrice = int.MaxValue;
                Website lowestRetailWebsite;
                Product lowestRetailProduct = null;

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
                            if (
                                ignoreList.Exists(
                                    i => i.GameURL.Contains(product.Url) || product.Url.Contains(i.GameURL)))
                                continue;

                            lowestRetailProduct = product;
                            lowestRetailWebsite = website;
                            LowestRetailPrice = product.Price;
                        }
                        else
                        {
                            LowestRetailPrice = int.MaxValue;
                        }
                    }
                    finally
                    {
                        if (LowestRetailPrice < listGame.GamePrice)
                        {
                            if (lowestRetailProduct != null)
                            {
                                if (MessageBox.Show(listGame.GameName, lowestRetailProduct.Price.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                                {
                                    System.Diagnostics.Process.Start(lowestRetailProduct.Url);
                                }

                            }
                            
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
            //websitesToSearch.Add(new Olx());
            //websitesToSearch.Add(new eBay());
            websitesToSearch.Add(new CEX());
            return websitesToSearch;
        }
    }
}