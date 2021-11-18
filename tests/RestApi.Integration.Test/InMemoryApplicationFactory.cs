using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApiRepo.Infrastructure;
using System;
using System.IO;

namespace RestApi.Integration.Test
{

    public class InMemoryApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        public void ClearTable<T>() where T : class
        {
            using var scope = this.Services.CreateScope(); ;
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            var set = db.Set<T>();
            set.RemoveRange(set);
            db.SaveChanges();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            builder
                .UseEnvironment("Testing")
                .UseSolutionRelativeContentRoot("")
                .ConfigureTestServices(services =>
                {
                    var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
                    services.AddScoped(serviceProvider => new DataContext(options));
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    CreateTestAdminUser(scopedServices);
                    EnsureDataCreated(scopedServices);
                })
                .UseConfiguration(config);
        }

        private void EnsureDataCreated(IServiceProvider scopedServices)
        {
            scopedServices.GetRequiredService<DataContext>().Database.EnsureCreated();
        }
        private void CreateTestAdminUser(IServiceProvider scopedServices)
        {
            var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
            var admin = new IdentityUser
            {
                Email = "test@integration.com",
                UserName = "test@integration.com"
            };
            if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                var adminRole = new IdentityRole("Admin");
                roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
            }
            userManager.CreateAsync(admin, "Pa$$word1").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
        }
    }
}
