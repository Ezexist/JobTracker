using MediatR;

namespace JobTracker.Application.Features.Subscriptions.Create
{
    public sealed record CreateSubscriptionCommand(
        string Name,
        bool RemoteOnly,
        int? MinSalary,
        string? Currency,
        List<string> Keywords,
        List<string> Locations) : IRequest<Guid>
    {
    }
}
