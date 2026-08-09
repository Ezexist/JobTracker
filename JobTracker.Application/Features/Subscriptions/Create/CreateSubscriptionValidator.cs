using FluentValidation;

namespace JobTracker.Application.Features.Subscriptions.Create
{
    public sealed class CreateSubscriptionValidator : AbstractValidator<CreateSubscriptionCommand>
    {
        public CreateSubscriptionValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x.Currency)
                .MaximumLength(3).WithMessage("Currency must not exceed 3 characters")
                .When(x => x.Currency is not null);

            RuleFor(x => x.MinSalary)
                .GreaterThan(0).WithMessage("Min salary must be greater than 0")
                .When(x => x.MinSalary.HasValue);

            RuleFor(x => x.Keywords)
                .NotEmpty().WithMessage("At least one keyword is required");

            RuleForEach(x => x.Keywords)
                .NotEmpty().WithMessage("Keyword must not be empty")
                .MaximumLength(100).WithMessage("Keyword must not exceed 100 characters");

            RuleForEach(x => x.Locations)
                .NotEmpty().WithMessage("Location must not be empty")
                .MaximumLength(100).WithMessage("Location must not exceed 100 characters");

        }
    }
}
