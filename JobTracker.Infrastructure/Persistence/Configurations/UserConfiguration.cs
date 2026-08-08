using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.Id)
                .ValueGeneratedNever();

            builder.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(user => user.Email)
                .IsUnique();

            builder.Property(user => user.CreatedAt)
                .IsRequired();
        }
    }
}
