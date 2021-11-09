using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rest_Api_Repo.Infrastructure.SchemaDefinitions;
using Rest_Api_Repo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using RestApiRepo.Domain.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace Rest_Api_Repo.Infrastructure
{
    public class DataContext : IdentityDbContext, IUnitOfWork
    {
        public const string DefaultSchema  = "post";
        public DbSet<Post> Posts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new PostEntitySchemaDefinition())
                .ApplyConfiguration(new RefreshTokenSchemaDefinition())
                .ApplyConfiguration(new TagSchemaDefinition())
                .ApplyConfiguration(new PostTagSchemaDefinition());

            base.OnModelCreating(modelBuilder);
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
