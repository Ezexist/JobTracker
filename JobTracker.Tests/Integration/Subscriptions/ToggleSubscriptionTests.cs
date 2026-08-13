using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace JobTracker.Tests.Integration.Subscriptions;

public class ToggleSubscriptionTests : IntegrationTestBase
{
    public ToggleSubscriptionTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_ReturnFalse_When_SubscriptionIsActive()
    {
        // Arrange
        var subscriptionId = await CreateSubscriptionAsync(".NET Jobs Kyiv");

        // Act
        var response = await HttpClient.PatchAsync(
            $"/api/subscriptions/{subscriptionId}/toggle",
            content: null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var isActive = await response.Content.ReadFromJsonAsync<bool>();
        isActive.Should().BeFalse();
    }

    [Fact]
    public async Task Should_ReturnTrue_When_SubscriptionWasToggledTwice()
    {
        // Arrange
        var subscriptionId = await CreateSubscriptionAsync(".NET Jobs Kyiv");

        // Первый toggle: true → false
        await HttpClient.PatchAsync($"/api/subscriptions/{subscriptionId}/toggle", content: null);

        // Act: второй toggle: false → true
        var response = await HttpClient.PatchAsync(
            $"/api/subscriptions/{subscriptionId}/toggle",
            content: null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var isActive = await response.Content.ReadFromJsonAsync<bool>();
        isActive.Should().BeTrue();
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_SubscriptionDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await HttpClient.PatchAsync(
            $"/api/subscriptions/{nonExistentId}/toggle",
            content: null);

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
}