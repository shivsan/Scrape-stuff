using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace NetUtils
{
    public static class Web
    {
        public static string GetHTTPResponse(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            System.Threading.Thread.Sleep(500);

            for (int retry = 0; retry < 3;)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.65 Safari/537.36";

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
                catch (Exception)
                {

                    retry++;
                }
            }

            return "";
        }
    }
}
