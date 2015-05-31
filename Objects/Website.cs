using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Websites.Sites
{
    public abstract class Website : IWebSite
    {
        public abstract Product GetProduct();
        public abstract string ParseProductName(string productName);
        public abstract string SearchURL(string productName);
        public string ProductName;
    }
}
