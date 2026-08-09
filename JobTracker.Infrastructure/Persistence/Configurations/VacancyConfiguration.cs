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
    public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.ToTable("Vacancies");

            builder.HasKey(vacancy => vacancy.Id);

            builder.Property(vacancy => vacancy.Id)
                .ValueGeneratedNever();

            builder.Property(vacancy => vacancy.Source)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(vacancy => vacancy.ExternalId)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(vacancy => vacancy.Title)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(vacancy => vacancy.Company)
                .HasMaxLength(200);

            builder.Property(vacancy => vacancy.Location)
                .HasMaxLength(200);

            builder.Property(vacancy => vacancy.Currency)
                .HasMaxLength(3);

            builder.Property(vacancy => vacancy.Url)
                .HasMaxLength(2048)
                .IsRequired();

            builder.Property(vacancy => vacancy.DetectedAt)
                .IsRequired();

            builder.HasIndex(vacancy => new
            {
                vacancy.Source,
                vacancy.ExternalId
            })
            .IsUnique();

            builder.HasIndex(vacancy => vacancy.Title);

            builder.HasIndex(vacancy => vacancy.IsRemote);
        }
    }
}
