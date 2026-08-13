using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace JobTracker.Tests.Integration.Subscriptions;

public class UpdateSubscriptionTests : IntegrationTestBase
{
    public UpdateSubscriptionTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_ReturnNoContent_When_UpdateIsValid()
    {
        // Arrange
        var subscriptionId = await CreateSubscriptionAsync(".NET Jobs Kyiv");

        var updateRequest = new
        {
            id = subscriptionId,
            name = ".NET Jobs Kyiv - Updated",
            remoteOnly = true,
            minSalary = 3000,
            currency = "USD",
            keywords = new[] { ".NET", "C#", "Azure" },
            locations = new[] { "Kyiv", "Remote" }
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("/api/subscriptions", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_UpdateSubscription_When_UpdateIsValid()
    {
        // Arrange
        var subscriptionId = await CreateSubscriptionAsync(".NET Jobs Kyiv");

        var updateRequest = new
        {
            id = subscriptionId,
            name = ".NET Jobs Remote",
            remoteOnly = true,
            minSalary = 5000,
            currency = "EUR",
            keywords = new[] { ".NET", "Azure" },
            locations = new[] { "Remote" }
        };

        // Act
        await HttpClient.PutAsJsonAsync("/api/subscriptions", updateRequest);

        // Проверяем, что изменения применились
        var getResponse = await HttpClient.GetAsync($"/api/subscriptions/{subscriptionId}");
        var subscription = await getResponse.Content.ReadFromJsonAsync<SubscriptionResponse>();

        // Assert
        subscription.Should().NotBeNull();
        subscription!.Name.Should().Be(".NET Jobs Remote");
        subscription.RemoteOnly.Should().BeTrue();
        subscription.MinSalary.Should().Be(5000);
        subscription.Currency.Should().Be("EUR");
        subscription.Keywords.Should().BeEquivalentTo(new[] { ".NET", "Azure" });
        subscription.Locations.Should().BeEquivalentTo(new[] { "Remote" });
    }

    [Fact]
    public async Task Should_ReturnBadRequest_When_NameIsEmpty()
    {
        // Arrange
        var subscriptionId = await CreateSubscriptionAsync(".NET Jobs");

        var updateRequest = new
        {
            id = subscriptionId,
            name = "",
            remoteOnly = false,
            minSalary = 2000,
            currency = "USD",
            keywords = new[] { ".NET" },
            locations = new[] { "Kyiv" }
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("/api/subscriptions", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_SubscriptionDoesNotExist()
    {
        // Arrange
        var updateRequest = new
        {
            id = Guid.NewGuid(),
            name = ".NET Jobs",
            remoteOnly = false,
            minSalary = 2000,
            currency = "USD",
            keywords = new[] { ".NET" },
            locations = new[] { "Kyiv" }
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("/api/subscriptions", updateRequest);

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