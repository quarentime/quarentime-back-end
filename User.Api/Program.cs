using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace User.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.ConfigureAppConfiguration((builderContext, config) =>
                     {
                         var env = builderContext.HostingEnvironment;
                         config.AddJsonFile("appsettings.json", optional: false)
                               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                     });

                     webBuilder.UseStartup<Startup>();
                     webBuilder.UseUrls("http://+:8080");
                 })
                 .ConfigureLogging(logging =>
                 {
                     logging.ClearProviders();
                 })
                 .UseNLog();
    }
}
