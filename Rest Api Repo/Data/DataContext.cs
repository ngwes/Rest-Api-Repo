using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rest_Api_Repo.Data.SchemaDefinitions;
using Rest_Api_Repo.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rest_Api_Repo.Data
{
    public class DataContext : IdentityDbContext
    {
        public const string DefaultSchema  = "post";
        public DbSet<Post> Posts { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostEntitySchemaDefinition());
            base.OnModelCreating(modelBuilder);
        }
    }
}
