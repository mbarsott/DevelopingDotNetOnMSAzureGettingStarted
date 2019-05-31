using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PsHelloAzure.Data;

namespace PsHelloAzure
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //            CreateWebHostBuilder(args).Build().Run();
            var host = BuildWebHost(args);
            MigrateDatabase(host);
            host.Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .Build();

        private static void MigrateDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>();
    }
}
