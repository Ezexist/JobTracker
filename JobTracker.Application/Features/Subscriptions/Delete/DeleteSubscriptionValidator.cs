using FluentValidation;

namespace JobTracker.Application.Features.Subscriptions.Delete
{
    public class DeleteSubscriptionValidator : AbstractValidator<DeleteSubscriptionCommand>
    {
        public DeleteSubscriptionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID is required");
        }
    }
}
