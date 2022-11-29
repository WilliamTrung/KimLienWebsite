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
        public static List<string> GetPictureId(string pictures)
        {
            //sample
            //https://res.cloudinary.com/dvqruuvxs/image/upload/v1669643226/
            List<string> result = new List<string>();
            foreach(var picture in pictures.Split(","))
            {
                var split = picture.Split("/");
                var id = split[7].Split(".")[0];
                result.Add(id);
            }
            return result;
        }
    }
}
