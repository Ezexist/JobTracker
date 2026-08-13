using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace JobTracker.Tests.Integration.Subscriptions
{
    public class GetSubscriptionListTests : IntegrationTestBase
    {
        public GetSubscriptionListTests(DatabaseFixture fixture) : base(fixture) 
        {
            
        }

        [Fact]
        public async Task Should_ReturnEmpty_When_NoSubscriptions()
        {
            //Act
            var response = await HttpClient.GetAsync("/api/subscriptions");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var subscriptions = await response.Content.ReadFromJsonAsync<List<SubscriptionResponse>>();
            subscriptions.Should().NotBeNull();
            subscriptions.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_ReturnSubscriptions_When_SubscriptionsExist()
        {
            //arrange
            await CreateSubscriptionsAsync(".Net Test name");
            await CreateSubscriptionsAsync("C++ job Lviv");

            //Act
            var response = await HttpClient.GetAsync("/api/subscriptions");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var sub = await response.Content.ReadFromJsonAsync<List<SubscriptionResponse>>();
            sub.Should().NotBeNull();
            sub.Should().HaveCount(2);
            
        }

        private async Task<Guid> CreateSubscriptionsAsync(string name)
        {
            var request = new
            {
                name = name,
                remoteOnly = false,
                minSalary = 2000,
                currency = "USD",
                keywords = new[] { ".NET" },
                locations = new[] { "Kyiv" }
            };

            var response = await HttpClient.PostAsJsonAsync("/api/subscriptions", request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        private sealed record SubscriptionResponse(
        Guid Id,
        string Name,
        bool IsActive,
        bool RemoteOnly,
        int? MinSalary,
        string? Currency,
        List<string> Keywords,
        List<string> Locations,
        DateTimeOffset CreatedAt);
    }
}
