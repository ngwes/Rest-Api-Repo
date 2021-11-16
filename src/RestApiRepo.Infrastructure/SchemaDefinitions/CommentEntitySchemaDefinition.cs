using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
 
using RestApiRepo.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Infrastructure.SchemaDefinitions
{
    public class CommentEntitySchemaDefinition : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comment", DataContext.DefaultSchema);
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Content);
            builder.Property(c => c.CreateAt);

            builder
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            builder
                .HasOne(c => c.Post)
                .WithMany()
                .HasForeignKey(c => c.PostId);
        }
    }
}
