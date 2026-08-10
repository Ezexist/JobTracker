using FluentAssertions;
using JobTracker.Application.Features.Subscriptions.Toggle;

namespace JobTracker.Tests.Unit.Subscriptions
{
    public class ToggleSubscriptionValidatorTests
    {
        private readonly ToggleSubscriptionValidator _validator;

        public ToggleSubscriptionValidatorTests()
        {
            _validator = new ToggleSubscriptionValidator();
        }

        [Fact]
        public void Should_PassValidation_When_IdIsValid()
        {
            //Arrange
            var command = new ToggleSubscriptionCommand(Guid.NewGuid());
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
            var command = new ToggleSubscriptionCommand(Guid.Empty);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
        }
    }
}
