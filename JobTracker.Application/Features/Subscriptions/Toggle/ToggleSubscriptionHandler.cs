using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Common.Exceptions;
using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace JobTracker.Application.Features.Subscriptions.Toggle
{
    public sealed class ToggleSubscriptionHandler
        : IRequestHandler<ToggleSubscriptionCommand, bool>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;

        public ToggleSubscriptionHandler(IAppDbContext appDbContext, ICurrentUserProvider currentUserProvider)
        {
            _dbContext = appDbContext;
            _currentUserProvider = currentUserProvider;
            
        }
        public async Task<bool> Handle(
            ToggleSubscriptionCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserProvider.UserId;

            var subscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(
                s => s.Id == request.Id && s.UserId == userId,
                cancellationToken);

            if (subscription == null)
            {
                throw new NotFoundException(nameof(Subscription), request.Id);
            }

            subscription.Toggle();
            subscription.Touch(DateTimeOffset.UtcNow);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return subscription.IsActive;
        }
    }
}
