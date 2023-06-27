using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Infrastructure.Configuration;

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Expire).IsRequired();
        builder.Property(x => x.RefreshToken).IsRequired().IsFixedLength().HasMaxLength(64);
        builder.HasIndex(x => x.RefreshToken).IsUnique();
    }
}