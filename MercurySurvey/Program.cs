using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace MercurySurvey
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            //生产
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();


            ////本地调试
            //IConfiguration Configuration = new ConfigurationBuilder()
            //           .SetBasePath(Environment.CurrentDirectory)
            //           .AddJsonFile("appsettings.json")
            //           .Build();

            ////josn文件存储形式，发布后可以修改
            //string httpUrl = Configuration["Url:Http"];

            //return WebHost.CreateDefaultBuilder(args)
            //    .UseStartup<Startup>()
            //    .UseUrls(httpUrl);
        }
    }
}
