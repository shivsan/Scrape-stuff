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

namespace Scrape_items_to_buy.Websites.Sites
{
    //http://www.snapdeal.com/search/autoSuggestion?q=assassins+creed+revelations&catId=0&ver=3
    public class SnapDeal : Website
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
            return string.Format("http://www.snapdeal.com/search?keyword={0}&catId=&categoryId=2426&suggested=false&vertical=p&noOfResults=20&clickSrc=searchOnSubCat&prodCatId=&changeBackToAll=false&foundInAll=false&categoryIdSearched=&cityPageUrl=&url=&utmContent=&catalogID=", ParseProductName(productName));
        }

        public string GetPageURL(string htmlBody)
        {
            //*[@id="675441"]/div/div[5]/div[1]/div/div[1]/div/a[1]
            var products = new List<Product>();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;

            //Get all pages as SnapDeal's search is not so great
            //var nodes = root.SelectNodes("//*[@id=\"content_wrapper\"]/div[3]/div[2]")[0].SelectNodes("//div[@class=\"product-title\"]");
            var nodes = root.SelectNodes("//div[@class=\"hoverProductWrapper product-txtWrapper  \"]");

            foreach(var node in nodes)
            {
                var title = node.SelectSingleNode("div[1]").InnerText.Trim();

                if (title.ToUpper().Contains("PS4") || title.ToUpper().Contains("PLAYSTATION")||title.ToUpper().Contains("PLAY STATION") || title.ToUpper().Contains("SONY"))
                {
                    var urlNode = node.SelectSingleNode("div[1]").ChildNodes["a"];
                    var gameTitle = urlNode.InnerText.Trim();
                                        
                    var url = urlNode.Attributes["href"].Value;
                    var price = Convert.ToInt32((node.SelectSingleNode("a/div[2]/div/span")).InnerHtml.Replace("Rs","").Trim());

                    products.Add(new Product()
                    {
                        RetailName = gameTitle,
                        Url = url,
                        Price = price
                    });
                }
            }

            var bestProduct = NameMatching.BestProduct(products, ProductName, 0.8, this);
            return bestProduct != null? bestProduct.Url : string.Empty;
        }

        public override int GetPrice(string htmlBody)
        {

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;
            var node = root.SelectSingleNode("//*[@id=\"selling-price-id\"]");
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
