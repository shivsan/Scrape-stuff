using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NameUtils
{
    public static class NameReplace
    {
        public static string ReplacePunctuation(string name, string replace)
        {
            Regex regex = new Regex(@"[\p{P}]");
            return regex.Replace(name, " ");
        }
    }
}
