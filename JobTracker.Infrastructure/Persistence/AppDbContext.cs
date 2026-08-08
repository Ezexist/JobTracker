
using JobTracker.Application.Common.Abstractions;
using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : DbContext(options), IAppDbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();

        public DbSet<SubscriptionKeyword> SubscriptionKeywords => Set<SubscriptionKeyword>();

        public DbSet<SubscriptionLocation> SubscriptionLocations => Set<SubscriptionLocation>();

        public DbSet<Vacancy> Vacancies => Set<Vacancy>();

        public DbSet<SubscriptionMatch> SubscriptionMatches => Set<SubscriptionMatch>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
