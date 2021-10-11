using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rest_Api_Repo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Data.SchemaDefinitions
{
    public class RefreshTokenSchemaDefinition : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken", DataContext.DefaultSchema);
            builder.HasKey(t => t.Token);
            builder.Property(t => t.JwtId);
            builder.Property(t => t.Invalidated);
            builder.Property(t => t.ExpiryDate);
            builder.Property(t => t.CreationDate);
            builder.Property(t => t.Used);

            builder
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);
        }
    }
}
