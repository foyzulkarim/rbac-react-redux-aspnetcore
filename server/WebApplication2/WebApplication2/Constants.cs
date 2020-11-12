using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebApplication2
{
    public class Constants
    {
        public static string AuthServer { get; private set; }

        public Constants(IConfiguration config)
        {
            var s = config["AuthServer"];
            AuthServer = s;
        }
    }
}
