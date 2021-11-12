using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Rest_Api_Repo.Infrastructure;
using Rest_Api_Repo.Extensions;
using Rest_Api_Repo.Installers;
using Rest_Api_Repo.Options;
using RestApi_Contracts.HealtCheck;
using System;
using System.Linq;
using System.Threading.Tasks;
using Rest_Api_Repo.Domain.MappingProfiles;

namespace Rest_Api_Repo
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
            IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(Configuration, Env);
            services.AddAutoMapper(new[] { typeof(DomainToResponseProfile), typeof(RequestToDomainProfile) });
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
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(x => new HealthCheck
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description ?? "healthy",
                        }),
                        Duration = report.TotalDuration
                    };
                    await context.Response.Body.WriteAsync(response.ToByte());
                }
            });
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(options =>
            {
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


        private async Task CreateAdminAsync(RoleManager<IdentityRole> roleManager)
        {

            var retry = Policy.Handle<Exception>()
                .WaitAndRetryAsync(new TimeSpan[]
                {
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(6),
                    TimeSpan.FromSeconds(12)
                });
            await retry.ExecuteAsync(async () =>
            {

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
