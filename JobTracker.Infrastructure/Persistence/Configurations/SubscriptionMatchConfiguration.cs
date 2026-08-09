using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Infrastructure.Persistence.Configurations
{
    public class SubscriptionMatchConfiguration : IEntityTypeConfiguration<SubscriptionMatch>
    {
        public void Configure(EntityTypeBuilder<SubscriptionMatch> builder)
        {
            builder.ToTable("SubscriptionMatches");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.MatchedAt)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.HasOne(match => match.Subscription)
                .WithMany(subscription => subscription.Matches)
                .HasForeignKey(match => match.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(match => match.Vacancy)
                .WithMany(vacancy => vacancy.Matches)
                .HasForeignKey(match => match.VacancyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(match => new
            {
                match.SubscriptionId,
                match.VacancyId
            });

            builder.HasIndex(match => match.Status);
        }
    }
}
