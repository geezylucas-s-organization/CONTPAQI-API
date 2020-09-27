using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CONTPAQ_API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CONTPAQ_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SDKServices.Conectar();
            //PlantillasServices.initializeHangfire();
            //PlantillasServices.func();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
