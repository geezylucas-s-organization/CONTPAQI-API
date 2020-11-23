using System;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CONTPAQ_API.Services
{
    public class DatabaseServices
    {
        public static IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json").Build();
            return builder;
        }

 

        public static string GetConnString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration.GetConnectionString("PlantillasDatabase");
        }
    }
}