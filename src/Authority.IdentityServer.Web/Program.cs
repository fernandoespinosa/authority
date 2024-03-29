﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Https;

namespace Authority.IdentityServer.Web
{
    public class Program
    {
        public static string Title => Assembly.GetEntryAssembly().GetName().Name;

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel(options => options.UseHttps("ssl/Authority.IdentityServer.Web.Local.pfx", "pass"))
                .UseUrls("https://Authority.IdentityServer.Web.Local")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
