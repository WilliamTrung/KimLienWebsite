using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Config.AuthConfig
{
    public static class CustomClaims
    {
        public static string ID { get; } = "Id";
        public static string Role { get; } = "Role";
    }
}
