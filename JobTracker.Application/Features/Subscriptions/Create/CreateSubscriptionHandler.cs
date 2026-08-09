using JobTracker.Application.Common.Abstractions;
using JobTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Application.Features.Subscriptions.Create
{
    public sealed class CreateSubscriptionHandler
        : IRequestHandler<CreateSubscriptionCommand, Guid>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;

        public CreateSubscriptionHandler(
            IAppDbContext dbContext,
            ICurrentUserProvider currentUserProvider)
        {
            _dbContext = dbContext;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<Guid> Handle(
            CreateSubscriptionCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserProvider.UserId;

            var subscription = new Subscription
            {
                UserId = userId,
                Name = request.Name,
                RemoteOnly = request.RemoteOnly,
                MinSalary = request.MinSalary,
                Currency = request.Currency,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
            };

            foreach( var keyword in request.Keywords)
            {
                subscription.Keywords.Add(new SubscriptionKeyword
                {
                    SubscriptionId = subscription.Id,
                    Value = keyword.Trim()
                });
            }

            foreach(var location in request.Locations)
            {
                subscription.Locations.Add(new SubscriptionLocation
                {
                    SubscriptionId = subscription.Id,
                    Value = location.Trim()
                });
            }

            _dbContext.Subscriptions.Add(subscription);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return subscription.Id;
        }
    }
}
