using FluentAssertions;
using JobTracker.Application.Features.Subscriptions.Update;

namespace JobTracker.Tests.Unit.Subscriptions
{
    public class UpdateSubscriptionValidatorTests
    {
        private readonly UpdateSubscriptionValidator _validator;

        public UpdateSubscriptionValidatorTests()
        {
            _validator = new UpdateSubscriptionValidator();
        }

        [Fact]
        public void Should_PassValidation_When_CommandIsValid()
        {
            // Arrange
            var command = new UpdateSubscriptionCommand(
                Id: Guid.NewGuid(),
                Name: ".NET Jobs Kyiv",
                RemoteOnly: false,
                MinSalary: 2000,
                Currency: "USD",
                Keywords: new List<string> { ".NET", "C#" },
                Locations: new List<string> { "Kyiv" });

            //Act
            var result = _validator.Validate(command);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Should_FailValidation_When_IdIsEmpty()
        {
            // Arrange
            var command = new UpdateSubscriptionCommand(
                Id: Guid.Empty,
                Name: ".NET Jobs",
                RemoteOnly: false,
                MinSalary: 2000,
                Currency: "USD",
                Keywords: new List<string> { ".NET" },
                Locations: new List<string> { "Kyiv" });

            //Act
            var result = _validator.Validate(command);
            //
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
        }
        [Fact]
        public void Should_FailValidation_When_NameIsEmpty()
        {
            // Arrange
            var command = new UpdateSubscriptionCommand(
                Id: Guid.NewGuid(),
                Name: "",
                RemoteOnly: false,
                MinSalary: 2000,
                Currency: "USD",
                Keywords: new List<string> { ".NET" },
                Locations: new List<string> { "Kyiv" });

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Should_FailValidation_When_KeywordsIsEmpty()
        {
            // Arrange
            var command = new UpdateSubscriptionCommand(
                Id: Guid.NewGuid(),
                Name: ".NET Jobs",
                RemoteOnly: false,
                MinSalary: 2000,
                Currency: "USD",
                Keywords: new List<string>(),
                Locations: new List<string> { "Kyiv" });

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Keywords");
        }
    }
}
