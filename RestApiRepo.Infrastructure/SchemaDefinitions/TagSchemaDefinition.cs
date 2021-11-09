using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rest_Api_Repo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Infrastructure.SchemaDefinitions
{
    public class TagSchemaDefinition : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tag", DataContext.DefaultSchema);
            builder
                .HasOne(t => t.UserCreator)
                .WithMany()
                .HasForeignKey(t => t.UserCreatorId);
            builder.Property(t => t.CreatedAt);
            builder.Property(t=>t.TagName);
        }
    }
}
