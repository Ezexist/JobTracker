using JobTracker.Application.Common.Abstractions;
using JobTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using JobTracker.Application.Common.Models;

namespace JobTracker.Application.Features.Subscriptions.List
{
    public sealed class GetSubscriptionListHandler
        : IRequestHandler<GetSubscriptionListQuery, List<SubscriptionDto>>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;

        public GetSubscriptionListHandler(IAppDbContext dbContext, ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
            _dbContext = dbContext;
        }

        public async Task<List<SubscriptionDto>> Handle(
            GetSubscriptionListQuery request, 
            CancellationToken cancellationToken)
        {
            var userId = _currentUserProvider.UserId;

            var subscriptions = await _dbContext.Subscriptions
                .AsNoTracking()
                .Where(subscription => subscription.UserId == userId)
                .OrderByDescending(subscription => subscription.CreatedAt)
                .Select(subscription => new SubscriptionDto(
                    subscription.Id,
                    subscription.Name,
                    subscription.IsActive,
                    subscription.RemoteOnly,
                    subscription.MinSalary,
                    subscription.Currency,
                    subscription.Keywords.Select(keyword => keyword.Value).ToList(),
                    subscription.Locations.Select(location => location.Value).ToList(),
                    subscription.CreatedAt
                    ))
                .ToListAsync(cancellationToken);

            return subscriptions;
        }
    }
}
