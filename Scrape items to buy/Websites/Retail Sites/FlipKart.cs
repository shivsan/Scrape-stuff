using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using NetUtils;
using HtmlAgilityPack;
using Objects.Websites;
using Objects.Websites.Sites;

namespace Scrape_items_to_buy.Websites.Sites
{
    public class FlipKart : Website
    {
        public override Product GetProduct()
        {
            var response = Web.GetHTTPResponse(SearchURL(ProductName));
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
            return HttpUtility.UrlEncode(productName);
        }

        public override string SearchURL(string productName)
        {
            //This is in the game section, sorted by price
            return string.Format("http://www.flipkart.com/games/pr?p%5B%5D=sort%3Dprice_asc&sid=4rr%2Ctg9&q={0}", ParseProductName(productName));
        }

        public string GetPageURL(string htmlBody)
        {
            ////*[@id="products"]/div/div[1]/div[1]
            //*[@id="products"]/div/div[1]/div[1]/div/div[2]/div[1]/a
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;
            var nodes = root.SelectNodes("//*[@id=\"products\"]/div/div[1]/div[1]/div/div[2]/div[1]/a");

            foreach(var node in nodes)
            {
                var title = node.Attributes["title"].Value;

                if(title.Contains("PS3"))
                {
                    return "http://www.flipkart.com" + node.Attributes["href"].Value;
                }

            }

            return string.Empty;
        }

        public int GetPrice(string htmlBody)
        {
            //*[@id=\"fk-mainbody-id\"]/div/div[8]/div/div[3]/div/div/div[4]/div/div[2]/div[1]/div/div[1]/div/div[1]/div/span[1]
            //*[@id="fk-mainbody-id"]/div/div[8]/div/div[3]/div/div/div[4]/div/div[2]/div[1]/div/div[1]/div/div[1]/span[1]
            //*[@id="fk-mainbody-id"]/div/div[8]/div/div[3]/div/div/div[4]/div/div[2]/div[1]/div/div[1]/div/div[1]/div/span[1]
            //root.SelectSingleNode("//*[@id=\"fk-mainbody-id\"]/div/div[8]/div/div[3]/div/div/div[4]/div/div[2]/div[1]/div/div[1]/div")

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;
            var nodes = root.SelectSingleNode("//span[@class='price'] ");
            var price = Convert.ToInt32(nodes.InnerHtml.Replace("Rs. ","").Replace(",",""));
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
