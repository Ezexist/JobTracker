using MediatR;

namespace JobTracker.Application.Features.Subscriptions.Delete
{
    public sealed record DeleteSubscriptionCommand (Guid Id) : IRequest;

}
