using MediatR;
using JobTracker.Application.Common.Models;
namespace JobTracker.Application.Features.Subscriptions.List
{
    public sealed record GetSubscriptionListQuery : IRequest<List<SubscriptionDto>>
    {
    }
}
