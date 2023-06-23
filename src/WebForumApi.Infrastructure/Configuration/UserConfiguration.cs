using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;
using WebForumApi.Domain.Entities.Common;
using BC = BCrypt.Net.BCrypt;

namespace WebForumApi.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion<UserId.EfCoreValueConverter>();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(254);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.Username).IsRequired().HasMaxLength(254);
        builder.HasIndex(x => x.Username).IsUnique();
        builder.Property(x => x.About).HasMaxLength(254);
        builder.Property(x => x.Location).HasMaxLength(254);
        builder.Property(x => x.Avatar).HasMaxLength(254);
        builder.Property(x => x.Role).HasMaxLength(31);
        builder.Property(x => x.About).HasMaxLength(254);
        builder.HasOne(x => x.Token).WithOne(x => x.User).HasForeignKey<Token>(u => u.UserId);
        builder.ToTable("user");
    }
}