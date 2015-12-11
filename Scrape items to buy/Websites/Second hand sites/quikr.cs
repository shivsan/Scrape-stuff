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
    public class quikr : Website
    {
        public override Product GetProduct()
        {
            var response = Web.GetHTTPResponse(SearchURL(ProductName));

            if(response.Contains("Unfortunately, there are no ads in this category right now."))
            {
                //Remove the price sort                
                response = Web.GetHTTPResponse(SearchURLNoSort(ProductName));
            }

            string pageURL = GetPageURL(response);
            var product = new Product();

            if (!string.IsNullOrEmpty(pageURL))
            {
                var webPageData = Web.GetHTTPResponse(pageURL);
                product.Price = GetPrice(webPageData);
                product.Url = pageURL;
            }
            
            //product.RetailName = GetProductTitle(webPageData);
            return product;
        }

        public override string ParseProductName(string productName)
        {
            productName = NameReplace.ReplacePunctuation(productName, " ");
            return HttpUtility.UrlEncode(productName.Replace(" ", "-"));
        }

        public override string SearchURL(string productName)
        {
            //This is in the game section, sorted by price
            return string.Format("http://bangalore.quikr.com/{0}-video-games-consoles/{0}/x23?order=priceAsc", ParseProductName(productName).ToLower());
        }

        public string SearchURLNoSort(string productName)
        {
            //This is in the game section, sorted by price
            return string.Format("http://bangalore.quikr.com/{0}-video-games-consoles/{0}/x23", ParseProductName(productName).ToLower());
        }

        public string GetPageURL(string htmlBody)
        {
            ////div[@id=\"a1d1container\"]

            var products = new List<Product>();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;

            //Get all pages as quikr's search is not so great            
            var adNode = root.SelectSingleNode("//*[@id=\"a1d1container\"]");

            var adNodes = adNode.SelectNodes("//div[@class=\"snb_entire_ad ad \"]");

            foreach (var node in adNodes)
            {
                //*[@id="ad210808315"]/div[1]/div[2]/div[1]/h3/a
                //*[@id="ad214637183"]/div[1]/div[2]/div[1]/h3/a
                //a[@class='adttllnk unbold']
                //div[1] / div[2] / div[1] / h3 / a
                var ad = node.SelectSingleNode("//h3[@class='adtitlesnb translate']");
                var title = ad.InnerText;

                if (title.ToUpper().Contains("PS4") || title.ToUpper().Contains("PLAYSTATION")||title.ToUpper().Contains("PLAY STATION") || title.ToUpper().Contains("SONY"))
                {
                    var urlNode = ad.SelectSingleNode("a");
                    var gameTitle = urlNode.Attributes["title"].Value.Trim();
                    
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

        public override double GetPrice(string htmlBody)
        {

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlBody);
            HtmlNode root = doc.DocumentNode;
            var node = root.SelectSingleNode("//span[@class=\"price\"]");
            var price = Convert.ToInt32(node.InnerHtml.Replace("Rs.", "").Replace(",", "").Trim());
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
