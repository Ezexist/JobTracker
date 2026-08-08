using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Infrastructure.Persistence.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");

            builder.HasKey(subscription => subscription.Id);

            builder.Property(subscription => subscription.Id)
                .ValueGeneratedNever();

            builder.Property(subscription => subscription.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(subscription => subscription.Currency)
                .HasMaxLength(3);

            builder.Property(subscription => subscription.CreatedAt)
                .IsRequired();

            builder.Property(subscription => subscription.UpdatedAt)
                .IsRequired();

            builder.HasOne(subscription => subscription.User)
                .WithMany(user => user.Subscriptions)
                .HasForeignKey(subscription => subscription.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(subscription => subscription.UserId);

            builder.HasIndex(subscription => subscription.IsActive);
        }
    }
}
