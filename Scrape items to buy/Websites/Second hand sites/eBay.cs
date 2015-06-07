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
    public class eBay : Website
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
            return string.Format("http://www.ebay.in/sch/Video-Computer-Games-/176299/i.html?_nkw={0}&_sop=15&LH_BIN=1", ParseProductName(productName));
        }

        public string GetPageURL(string htmlBody)
        {
            //*[@id=\"ResultSetItems\"]
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;
            var nodes = root.SelectNodes("//*[@id=\"ListViewInner\"]");
            
            var adNodes = root.SelectNodes("//h3[@class=\"lvtitle\"]");            

            foreach (var node in adNodes)
            {
                var aHrefChild = node.ChildNodes["a"];
                var title = aHrefChild.InnerText;

                if (title.ToUpper().Contains("PS3") || title.ToUpper().Contains("PLAYSTATION") || title.ToUpper().Contains("PLAY STATION") || title.ToUpper().Contains("SONY"))
                {
                    return aHrefChild.Attributes["href"].Value;
                }

            }
            return string.Empty;
        }

        public override int GetPrice(string htmlBody)
        {
            //*[@id="prcIsum"]
            //*[@id="prcIsum"]
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;
            var nodes = root.SelectSingleNode("//*[@id=\"prcIsum\"]");
            var price = Convert.ToInt32(Convert.ToDecimal(nodes.InnerHtml.Replace("Rs.", "").Replace(",", "").Replace(" ", "")));
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
