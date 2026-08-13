using JobTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace JobTracker.Tests.Integration
{
    public sealed class DatabaseFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container;

        public string ConnectionString { get;private set; } = string.Empty;

        public DatabaseFixture()
        {
            _container = new PostgreSqlBuilder("postgres:17")
                .WithDatabase("jobtracker_base")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();
        }
        public async Task InitializeAsync()
        {
            await _container.StartAsync();

            ConnectionString = _container.GetConnectionString();

            await ApplyMigrationAsync();
        }

        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }
        private async Task ApplyMigrationAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

            await using var dbContext = new AppDbContext(options);

            await dbContext.Database.MigrateAsync();
        }


    }
}
