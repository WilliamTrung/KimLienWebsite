using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppService.Extension
{
    public static class Helper
    {
        public static bool MinimalCompareString(string a, string b)
        {
            return a.ToLower().Contains(b.ToLower());
        }
    }
}
