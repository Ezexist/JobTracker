using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace JobTracker.Tests.Integration.Subscriptions;

public class GetSubscriptionByIdTests : IntegrationTestBase
{
    public GetSubscriptionByIdTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_ReturnSubscription_When_SubscriptionExists()
    {
        // Arrange
        var subscriptionId = await CreateSubscriptionAsync(".NET Jobs Kyiv");

        // Act
        var response = await HttpClient.GetAsync($"/api/subscriptions/{subscriptionId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var subscription = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();
        subscription.Should().NotBeNull();
        subscription!.Id.Should().Be(subscriptionId);
        subscription.Name.Should().Be(".NET Jobs Kyiv");
        subscription.Keywords.Should().Contain(".NET");
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_SubscriptionDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/api/subscriptions/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<Guid> CreateSubscriptionAsync(string name)
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