using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApiRepo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.Infrastructure.SchemaDefinitions
{
    public class PostTagSchemaDefinition : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.ToTable("PostTag", DataContext.DefaultSchema);
            builder.HasKey(join => new { join.PostId, join.TagId});

            builder.HasOne(join => join.Post)
                .WithMany(p=>p.PostTags)
                .HasForeignKey(join=>join.PostId);

            builder.HasOne(join => join.Tag)
                .WithMany(t=>t.PostTags)
                .HasForeignKey(join => join.TagId);
        }

    }
}
