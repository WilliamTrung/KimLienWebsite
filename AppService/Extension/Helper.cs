using AppService.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public static string MergeListString(IList<string> list)
        {
            var result = "";
            foreach(var item in list){
                if(item != "")
                    result += item + ",";
            }
            return result;
        }
        public static bool Login(this ISession session, User user)
        {
            if(session != null)
            {
                var json = JsonSerializer.Serialize<User>(user);
                session.SetString("login-user", json);
                return true;
            }
            return false;
        }
        public static void Logout(this ISession session)
        {
            if(session != null)
            {
                session.SetString("login-user", null);
            }
        }
        public static User? GetLoginUser(this ISession session)
        {
            User? loginUser = null;
            if(session != null)
            {
                var user = session.GetString("login-user");
                if(user != null)
                {
                    loginUser = JsonSerializer.Deserialize<User>(user);
                }
            }
            return loginUser;
        }
    }
}
