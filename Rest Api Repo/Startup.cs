using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Rest_Api_Repo.Data;
using Rest_Api_Repo.Installers;
using Rest_Api_Repo.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(Configuration);
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            DataContext dataContext,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(options=> {
                options.RouteTemplate = swaggerOptions.JsonRoute;   
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });
            ExecuteMigrations(env, dataContext);
            CreateAdminAsync(roleManager).GetAwaiter().GetResult();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private async Task CreateAdminAsync(RoleManager<IdentityRole> roleManager) {

            var retry = Policy.Handle<Exception>()
                .WaitAndRetryAsync(new TimeSpan[]
                {
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(6),
                    TimeSpan.FromSeconds(12)
                });
            await retry.ExecuteAsync(async ()=> { 

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var adminRole = new IdentityRole("Admin");
                    await roleManager.CreateAsync(adminRole);
                }
                if (!await roleManager.RoleExistsAsync("Poster"))
                {
                    var posterRole = new IdentityRole("Poster");
                    await roleManager.CreateAsync(posterRole);
                }
            });
        }
        private void ExecuteMigrations(IWebHostEnvironment env, DataContext dataContext)
        {
            if (env.EnvironmentName == "Testing") return;

            var retry = Policy.Handle<SqlException>()
                .WaitAndRetry(new TimeSpan[]
                {
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(6),
                    TimeSpan.FromSeconds(12)
                });

            retry.Execute(() =>
                dataContext.Database.Migrate());
                
        }
    }
}
