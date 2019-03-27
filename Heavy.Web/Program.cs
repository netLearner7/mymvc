using System;
using System.IO;
using Heavy.Web.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Heavy.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                //默认最小记录级别
                .MinimumLevel.Debug()
                //如果遇到Microsoft命名空间，那么最小记录级别为Information
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                //通过上下文记录其他信息（具体什么不知道。。）
                .Enrich.FromLogContext()
                //写到控制台
                .WriteTo.Console()
                //写到文件，生成文件的间隔
                .WriteTo.File(Path.Combine( "logs","log.txt"), rollingInterval: RollingInterval.Day)
                //创建这个log
                .CreateLogger();

            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<HeavyContext>();
                    DatabaseInitializer.Seed(context);
                }
                catch (Exception)
                {
                    //we could log this in a real-world situation
                }
            }


            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSerilog()
                .UseStartup<Startup>();
    }
}
