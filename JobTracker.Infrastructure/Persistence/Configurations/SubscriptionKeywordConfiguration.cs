using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Infrastructure.Persistence.Configurations
{
    public class SubscriptionKeywordConfiguration : IEntityTypeConfiguration<SubscriptionKeyword>
    {
        public void Configure(EntityTypeBuilder<SubscriptionKeyword> builder)
        {
            builder.ToTable("SubscriptionKeywords");

            builder.HasKey(keyword => keyword.Id);

            builder.Property(keyword => keyword.Id)
                .ValueGeneratedNever();

            builder.Property(keyword => keyword.Value)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(keyword => keyword.Subscription)
                .WithMany(subscription => subscription.Keywords)
                .HasForeignKey(keyword => keyword.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(keyword => new
            {
                keyword.SubscriptionId,
                keyword.Value
            })
            .IsUnique();
        }
    }
}
