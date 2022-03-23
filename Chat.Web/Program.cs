using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using System;
using System.Security.Authentication;

using Util;

namespace Chat.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(kestrelOptions =>
                    {
                        kestrelOptions.Limits.MaxRequestBodySize = Convert.ToInt32(AppSettings.App("fileServer:singleFileMaxSize")) * 1024 * 1024;
                        kestrelOptions.ConfigureHttpsDefaults(s => s.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13);
                    });
                    webBuilder.UseStartup<Startup>();
                    if (args.Length > 0) {
                        webBuilder.UseUrls(args[0]);
                    }
                });
    }
}
