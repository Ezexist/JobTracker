using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Common.Exceptions;
using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace JobTracker.Application.Features.Subscriptions.Update
{
    public sealed class UpdateSubscriptionHandler
        : IRequestHandler<UpdateSubscriptionCommand>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;
        public UpdateSubscriptionHandler(IAppDbContext dbContext, ICurrentUserProvider currentUserProvider)
        {
            _dbContext = dbContext;
            _currentUserProvider = currentUserProvider;
        }
        public async Task Handle(
            UpdateSubscriptionCommand request, 
            CancellationToken cancellationToken)
        {
            var userId = _currentUserProvider.UserId;

            var subscription = await _dbContext.Subscriptions
                .Include(s => s.Keywords)
                .Include(s => s.Locations)
                .FirstOrDefaultAsync(
                s => s.Id == request.Id && s.UserId == userId,
                cancellationToken);

            if(subscription is null)
            {
                throw new NotFoundException(nameof(Subscription),request.Id);
            }

            subscription.Name = request.Name;
            subscription.RemoteOnly = request.RemoteOnly;
            subscription.MinSalary = request.MinSalary;
            subscription.Currency = request.Currency;
            subscription.Touch(DateTimeOffset.UtcNow);

            UpdateKeywords(subscription,request.Keywords);
            UpdateLocations(subscription, request.Locations);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private static void UpdateLocations(Subscription subscription, List<string> locations)
        {
            subscription.Locations.Clear();

            foreach(var location in locations)
            {
                subscription.Locations.Add(new SubscriptionLocation
                {
                    SubscriptionId = subscription.Id,
                    Value = location.Trim()
                });
            }
        }

        private static void UpdateKeywords(Subscription subscription, List<string> keywords)
        {
            subscription.Keywords.Clear();

            foreach(var keyword in keywords)
            {
                subscription.Keywords.Add(new SubscriptionKeyword
                {
                    SubscriptionId = subscription.Id,
                    Value = keyword.Trim()
                });
            }
        }
    }
}
