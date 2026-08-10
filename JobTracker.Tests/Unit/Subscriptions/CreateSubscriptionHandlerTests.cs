using FluentAssertions;
using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Features.Subscriptions.Create;
using JobTracker.Domain.Entities;
using JobTracker.Tests.Unit.Common;
using Microsoft.EntityFrameworkCore;
using Moq;
namespace JobTracker.Tests.Unit.Subscriptions
{
    public class CreateSubscriptionHandlerTests
    {
        private readonly Mock<IAppDbContext> _appDbContextMock;
        private readonly Mock<ICurrentUserProvider> _userProviderMock;
        private readonly CreateSubscriptionHandler _handler;

        private readonly Guid TestUsedId = Guid.NewGuid();

        public CreateSubscriptionHandlerTests()
        {
            _appDbContextMock = new Mock<IAppDbContext>();
            _userProviderMock = new Mock<ICurrentUserProvider>();

            _userProviderMock
                .Setup(x => x.UserId)
                .Returns(TestUsedId);

            var mockSubscriptions = MockDbSetHelper.CreateMockDbSet(
                new List<Subscription>().AsQueryable());

            _appDbContextMock
                .Setup(x => x.Subscriptions)
                .Returns(mockSubscriptions.Object);

            _handler = new CreateSubscriptionHandler(
                _appDbContextMock.Object,
                _userProviderMock.Object);
        }

        [Fact]
        public async Task Should_CreateSubscription_When_CommandIsValid()
        {
            //Arrange
            var command = new CreateSubscriptionCommand(
                Name: ".NET Jobs Kyiv",
                RemoteOnly: false,
                MinSalary: 2000,
                Currency: "USD",
                Keywords: new List<string> { ".NET", "C#" },
                Locations: new List<string> { "Kyiv" });

            //Act
            var result = await _handler.Handle(command,CancellationToken.None);
            //Assert
            result.Should().NotBeEmpty();

            _appDbContextMock.Verify(
                x => x.Subscriptions.Add(It.IsAny<Subscription>()),
                Times.Once);

            _appDbContextMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
        [Fact]
        public async Task Should_UseCurrentUserId_When_CreatingSubscription()
        {
            // Arrange
            var command = new CreateSubscriptionCommand(
                Name: ".NET Jobs",
                RemoteOnly: false,
                MinSalary: null,
                Currency: null,
                Keywords: new List<string> { ".NET" },
                Locations: new List<string> { "Remote" });

            Subscription? capturedSubscription = null;

            _appDbContextMock
                .Setup(x => x.Subscriptions.Add(It.IsAny<Subscription>()))
                .Callback<Subscription>(x => capturedSubscription = x);

            //Act
            await _handler.Handle(command, CancellationToken.None);
            //Assert
            capturedSubscription.Should().NotBeNull();
            capturedSubscription.UserId.Should().Be(TestUsedId);
        }
        [Fact]
        public async Task Should_CreateKeywordsAndLocations_When_CommandHasThem()
        {
            // Arrange
            var command = new CreateSubscriptionCommand(
                Name: ".NET Jobs",
                RemoteOnly: true,
                MinSalary: 3000,
                Currency: "USD",
                Keywords: new List<string> { ".NET", "C#", "Azure" },
                Locations: new List<string> { "Kyiv", "Remote" });

            Subscription? capturedSubscription = null;

            _appDbContextMock
                .Setup(x => x.Subscriptions.Add(It.IsAny<Subscription>()))
                .Callback<Subscription>(x => capturedSubscription = x);


            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            capturedSubscription.Should().NotBeNull();
            capturedSubscription!.Keywords.Count.Should().Be(3);
            capturedSubscription.Locations.Count.Should().Be(2);
        }
    }
}
