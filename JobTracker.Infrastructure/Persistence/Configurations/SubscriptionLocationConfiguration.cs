using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure.Persistence.Configurations
{
    public class SubscriptionLocationConfiguration : IEntityTypeConfiguration<SubscriptionLocation>
    {
        public void Configure(EntityTypeBuilder<SubscriptionLocation> builder)
        {
            builder.ToTable("SubscriptionLocations");

            builder.HasKey(location => location.Id);

            builder.Property(location => location.Id)
                .ValueGeneratedNever();

            builder.Property(location => location.Value)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(location => location.Subscription)
                .WithMany(subscription => subscription.Locations)
                .HasForeignKey(location => location.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(location => new
            {
                location.SubscriptionId,
                location.Value
            })
            .IsUnique();
        }
    }
}
