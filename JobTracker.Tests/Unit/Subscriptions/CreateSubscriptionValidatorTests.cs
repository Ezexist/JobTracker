using FluentAssertions;
using JobTracker.Application.Features.Subscriptions.Create;

namespace JobTracker.Tests.Unit.Subscriptions
{
    public class CreateSubscriptionValidatorTests
    {
        private readonly CreateSubscriptionValidator _validator;

        public CreateSubscriptionValidatorTests()
        {
            _validator = new CreateSubscriptionValidator();
        }
        [Fact]
        public void ShouldPass_Validation_When_Command_IsValid()
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
            var result = _validator.Validate(command);
            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Should_FailValidation_When_Command_IsValid()
        {
            //Arrange
            var command = new CreateSubscriptionCommand(
              Name: "",
              RemoteOnly: false,
              MinSalary: 2000,
              Currency: "USD",
              Keywords: new List<string> { ".NET" },
              Locations: new List<string> { "Kyiv" });
            //Act
            var result = _validator.Validate(command);
            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Should_FailValidation_When_NameExceed200Characters()
        {
            //Arrange
            var longName = new string('a', 201);

            var command = new CreateSubscriptionCommand(
              Name: longName,
              RemoteOnly: false,
              MinSalary: 2000,
              Currency: "USD",
              Keywords: new List<string> { ".NET" },
              Locations: new List<string> { "Kyiv" });

            //Act
            var result = _validator.Validate(command);
            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }
        [Fact]
        public void Should_FailValidation_When_MinSalaryisNegative()
        {
            // Arrange
            var command = new CreateSubscriptionCommand(
                Name: ".NET Jobs",
                RemoteOnly: false,
                MinSalary: -100,
                Currency: "USD",
                Keywords: new List<string> { ".NET" },
                Locations: new List<string> { "Kyiv" });

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "MinSalary");
        }
        public void Should_PassValidation_When_MinSalaryIsNull()
        {
            // Arrange
            var command = new CreateSubscriptionCommand(
                Name: ".NET Jobs",
                RemoteOnly: false,
                MinSalary: null,
                Currency: "USD",
                Keywords: new List<string> { ".NET" },
                Locations: new List<string> { "Kyiv" });

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
        public void Should_FailValidation_When_CurrencyExceeds3Characters()
        {
            // Arrange
            var command = new CreateSubscriptionCommand(
                Name: ".NET Jobs",
                RemoteOnly: false,
                MinSalary: 2000,
                Currency: "USDT",
                Keywords: new List<string> { ".NET" },
                Locations: new List<string> { "Kyiv" });

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Currency");
        }

    }
}
