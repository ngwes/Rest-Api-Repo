using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rest_Api_Repo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo
{
    public class Program
    {
        public static void /*async Task*/ Main(string[] args)
        {
            //var host = CreateHostBuilder(args).Build();
            //using (var serviceScope = host.Services.CreateScope())
            //{
            //    var env = serviceScope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            //    if (env.IsDevelopment())
            //    {
            //        var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
            //        await dbContext.Database.MigrateAsync();
            //    }
            //}
            CreateHostBuilder(args).Build().Run();
            //host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
