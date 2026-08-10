using FluentAssertions;
using JobTracker.Application.Features.Subscriptions.Delete;

namespace JobTracker.Tests.Unit.Subscriptions
{
    public class DeleteSubscriptionValidatorTests
    {
        private readonly DeleteSubscriptionValidator _validator;

        public DeleteSubscriptionValidatorTests()
        {
            _validator = new DeleteSubscriptionValidator();
        }

        [Fact]
        public void Should_PassValidation_When_IdIsValid()
        {
            //Arrange
            var command = new DeleteSubscriptionCommand(Guid.NewGuid());

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
            var command = new DeleteSubscriptionCommand(Guid.Empty);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id");
        }
    }
}
