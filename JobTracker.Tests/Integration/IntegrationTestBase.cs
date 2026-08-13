using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Common.CurrentUser;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JobTracker.Tests.Integration
{
    public abstract class IntegrationTestBase : IClassFixture<DatabaseFixture>, IAsyncLifetime
    {
        protected readonly HttpClient HttpClient;
        protected readonly DatabaseFixture DatabaseFixture;

        private readonly CustomWebApplicationFactory _factory;
        private Respawner _respawner = null!;
        public IntegrationTestBase(DatabaseFixture fixture)
        {
            DatabaseFixture = fixture;
            _factory = new CustomWebApplicationFactory(fixture.ConnectionString);
            HttpClient = _factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            if (_respawner is null)
            {
                await using var connection = new NpgsqlConnection(DatabaseFixture.ConnectionString);
                await connection.OpenAsync();

                _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
                {
                    DbAdapter = DbAdapter.Postgres
                });
            }

            await using var resetConnection = new NpgsqlConnection(DatabaseFixture.ConnectionString);
            await resetConnection.OpenAsync();

            await _respawner.ResetAsync(resetConnection);

            //Create user after db reset
            await SeedDefaultUser();

          
        }
        public Task DisposeAsync() => Task.CompletedTask;
        
        private async  Task SeedDefaultUser()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var currentUser = scope.ServiceProvider.GetRequiredService<ICurrentUserProvider>();

            var defaultUsedId = currentUser.UserId;

            var userExist = await dbContext.Users.AnyAsync(u => u.Id == defaultUsedId);

            if (!userExist)
            {
                var user = new User
                {
                    Id = defaultUsedId,
                    Email = "owner@local.com",
                    CreatedAt = DateTimeOffset.UtcNow
                };
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync(CancellationToken.None);             
            }          
        }
    }
}
