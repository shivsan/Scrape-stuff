using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NetUtils;
using HtmlAgilityPack;
using Objects.Websites;
using Objects.Websites.Sites;

namespace Scrape_items_to_buy.Websites.Sites
{
    public class CEX : Website
    {
        public override Product GetProduct()
        {
            var response = Web.GetHTTPResponse(SearchURL(ProductName)).Replace("\n", "");

            if (response.Equals("empty"))
                return null;

            string pageURL = GetPageURL(response);
            var product = new Product();
            //var webPageData = Web.GetHTTPResponse(pageURL);

            if (BlackListDescription(response))
                return null;

            product.Price = GetPrice(response);
            product.Url = pageURL;
            product.RetailName = GetProductTitle(response);
            return product;
        }

        public override string ParseProductName(string productName)
        {
            return HttpUtility.UrlEncode(productName);
        }

        public override string SearchURL(string productName)
        {
            //This is in the game section, sorted by price
            return string.Format("https://in.webuy.com/predictive/predict.php?q={0}&limit=10", ParseProductName(productName));
        }

        public string GetPageURL(string htmlBody)
        {
            //*[@id=\"ResultSetItems\"]
            var pageUrl = Regex.Replace(htmlBody, "%%.*", "");
            return string.Format("https://in.webuy.com/product.php?sku={0}", pageUrl);
        }

        public double GetPrice(string htmlBody)
        {
            //*[@id="prcIsum"]
            //*[@id="prcIsum"]
            //Playstation4 Software
            Regex regex = new Regex(@"Playstation4 Software");
            var costs = regex.Replace(htmlBody, " ");
            costs = Regex.Replace(costs, "%%", "|");
            var cost = costs.Trim().Split('|');
            var price = Convert.ToInt32(Convert.ToDouble(cost[6].Replace("Rs.", "").Replace(",", "").Replace(" ", "")));
            return base.GetPrice(price, "CEX");
        }

        public string GetProductTitle(string htmlBody)
        {
            var title = htmlBody.Replace("%%", "|").Split('|')[1];
            
            return title;
        }
    }
}
