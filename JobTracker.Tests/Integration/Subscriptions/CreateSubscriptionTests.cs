using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace JobTracker.Tests.Integration.Subscriptions
{
    public class CreateSubscriptionTests : IntegrationTestBase
    {
        public CreateSubscriptionTests(DatabaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public async Task Should_ReturnCreated_When_CommandIsValid()
        {
            //Arrange
            var request = new
            {
                name = ".Net Jobs Odesa",
                remoteOnly = false,
                minSalary = 1000,
                currency = "UDS",
                keywords = new[] { ".NET", "C#" },
                locations = new[] { "Odesa" }
            };

            //Act
            var response = await HttpClient.PostAsJsonAsync("/api/subscriptions", request);

            var responseBody = await response.Content.ReadAsStringAsync();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created,
                $"Response body{responseBody}");
        }

    }
}
