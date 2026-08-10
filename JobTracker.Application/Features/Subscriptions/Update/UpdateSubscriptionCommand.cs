using MediatR;

namespace JobTracker.Application.Features.Subscriptions.Update
{
    public sealed record UpdateSubscriptionCommand(
        Guid Id,
        string Name,
        bool RemoteOnly,
        int? MinSalary,
        string? Currency,
        List<string> Keywords,
        List<string> Locations) : IRequest;
}
