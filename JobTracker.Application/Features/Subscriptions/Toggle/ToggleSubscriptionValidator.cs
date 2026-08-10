using FluentValidation;

namespace JobTracker.Application.Features.Subscriptions.Toggle
{
    public sealed class ToggleSubscriptionValidator : AbstractValidator<ToggleSubscriptionCommand>
    {
        public ToggleSubscriptionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
