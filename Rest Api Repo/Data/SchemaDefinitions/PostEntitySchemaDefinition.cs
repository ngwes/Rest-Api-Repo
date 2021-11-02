using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rest_Api_Repo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Data.SchemaDefinitions
{
    public class PostEntitySchemaDefinition : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post", DataContext.DefaultSchema);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name);
            
            builder
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p=>p.UserId);
                
        }
    }
}
