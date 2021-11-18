using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Entitites;
using RestApiRepo.Domain.Repositories;
using RestApiRepo.Infrastructure.SchemaDefinitions;
using System.Threading;
using System.Threading.Tasks;

namespace RestApiRepo.Infrastructure
{
    public class DataContext : IdentityDbContext, IUnitOfWork
    {
        public const string DefaultSchema = "post";
        public DbSet<Post> Posts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
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
                .ApplyConfiguration(new PostTagSchemaDefinition())
                .ApplyConfiguration(new CommentEntitySchemaDefinition());

            base.OnModelCreating(modelBuilder);
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
