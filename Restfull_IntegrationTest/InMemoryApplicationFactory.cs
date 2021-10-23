using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rest_Api_Repo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restfull_IntegrationTest
{
    
    public class InMemoryApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        public void ClearTable<T>() where T :class
        {
            using var scope = this.Services.CreateScope(); ;
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            var set = db.Set<T>();
            set.RemoveRange(set);
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")
                .UseSolutionRelativeContentRoot("")
                .ConfigureTestServices(services =>
                {
                    var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                    services.AddScoped<DataContext>(serviceProvider => new DataContext(options));

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DataContext>();
                    db.Database.EnsureCreated();
                });
        }
    }
}
