using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Websites
{
    public interface IWebSite
    {
        Product GetProduct();
        string SearchURL(string productName);
        string ParseProductName(string productName);
    }
}
