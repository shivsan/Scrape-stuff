using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using NetUtils;
using HtmlAgilityPack;
using NameMatch;
using Objects.Websites;
using Objects.Websites.Sites;
using NameUtils;

namespace Scrape_items_to_buy.Websites.Sites
{
    //http://www.snapdeal.com/search/autoSuggestion?q=assassins+creed+revelations&catId=0&ver=3
    public class Olx : Website
    {
        public override Product GetProduct()
        {
            var response = Web.GetHTTPResponse(SearchURL(ProductName));

            if(response.ToLower().Contains("no results found"))
            {
                return null;
            }

            string pageURL = GetPageURL(response);
            var product = new Product();
            var webPageData = Web.GetHTTPResponse(pageURL);
            product.Price = GetPrice(webPageData);
            product.Url = pageURL;
            //product.RetailName = GetProductTitle(webPageData);
            return product;
        }

        public override string ParseProductName(string productName)
        {
            //productName = NameReplace.ReplacePunctuation(productName, " ");
            return HttpUtility.UrlEncode(productName.Replace(" ", "-"));
        }

        public override string SearchURL(string productName)
        {
            //This is in the game section, sorted by price
            return string.Format("http://olx.in/bangalore/games-consoles/q-{0}/?search%5Border%5D=filter_float_price%3Aasc", ParseProductName(productName));
        }

        public string GetPageURL(string htmlBody)
        {
            //*[@id="675441"]/div/div[5]/div[1]/div/div[1]/div/a[1]
            var products = new List<Product>();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;

            //var nodes = root.SelectNodes("//*[@id=\"content_wrapper\"]/div[3]/div[2]")[0].SelectNodes("//div[@class=\"product-title\"]");
            //Get all pages as quikr's search is not so great            
            //var adNode = root.SelectSingleNode("//*[@id=\"offer onclick\"]");
            //*[@id="offers_table"]/tbody/tr[2]/td/table/tbody/tr[1]/td[2]/h3/a
            //*[@id="offers_table"]/tbody/tr[3]/td/table/tbody/tr[1]/td[2]/h3/a
            // table/tbody/tr[1]/td[2]/h3/a
            var adNodes = root.SelectNodes("//td[@class=\"offer onclick  \"]");
            
            foreach (var node in adNodes)
            {
                var title = node.InnerText.Trim();

                if (title.ToUpper().Contains("PS3") || title.ToUpper().Contains("PLAYSTATION")||title.ToUpper().Contains("PLAY STATION") || title.ToUpper().Contains("SONY"))
                {
                    var urlNode = node.SelectSingleNode("//table/tbody/tr[1]/td[2]/h3/a");
                    var gameTitle = urlNode.ChildNodes["span"].InnerText.Trim();
                    
                    var url = urlNode.Attributes["href"].Value;
                    
                    products.Add(new Product()
                    {
                        RetailName = gameTitle,
                        Url = url
                    });
                }
            }

            var bestProduct = NameMatching.BestProduct(products, ProductName, 0.8);
            return bestProduct != null? bestProduct.Url : string.Empty;
        }

        public override int GetPrice(string htmlBody)
        {

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;
            var node = root.SelectSingleNode("//div[@class=\"pricelabel tcenter\"]/strong");            
            var price = Convert.ToInt32(node.InnerText);
            return price;
        }

        public string GetProductTitle(string htmlBody)
        {

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;
            var node = root.SelectSingleNode("//*[@id=\"selling-price-id\"]");
            var title = node.InnerText;
            return title;
        }
    }
}
