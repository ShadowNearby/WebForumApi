using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Infrastructure.Configuration;

public class FieldConfiguration : IEntityTypeConfiguration<Field>
{
    public void Configure(EntityTypeBuilder<Field> builder)
    {
        builder.HasMany(f => f.Users).WithMany(u => u.Fields).UsingEntity("UserField");
        builder.Property(q => q.Content).IsRequired().HasMaxLength(128);
    }
}