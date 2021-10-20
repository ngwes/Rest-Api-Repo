using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rest_Api_Repo.Data;
using Rest_Api_Repo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITagService, TagService>();
            //services.AddSingleton<IPostService, CosmosPostService>();
        }
    }
}
