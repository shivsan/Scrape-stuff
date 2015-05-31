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
            return string.Format("http://www.amazon.in/s/ref=nb_sb_noss_2?url=node%3D1376877031&field-keywords={0}", ParseProductName(productName));
        }

        public string GetPageURL(string htmlBody)
        {
            ////*[@id="products"]/div/div[1]/div[1]
            //*[@id="atfResults"]
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;
            var nodes = root.SelectNodes("//*[@id=\"atfResults\"]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var aHrefChild = node.SelectSingleNode("//*[@id=\"result_0\"]/div/div/div/div[2]/div[1]").ChildNodes["a"];
                    var title = aHrefChild.Attributes["title"].Value;

                    if (title.Contains("PS3"))
                    {
                        return aHrefChild.Attributes["href"].Value;
                    }

                }
            }
            else
            {

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
            var nodes = root.SelectSingleNode("//*[@id=\"priceblock_ourprice\"]/text()");
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
