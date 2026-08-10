using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Common.Exceptions;
using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace JobTracker.Application.Features.Subscriptions.Delete
{
    public sealed class DeleteSubscriptionHandler : IRequestHandler<DeleteSubscriptionCommand>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;

        public DeleteSubscriptionHandler(IAppDbContext appDbContext,ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
            _dbContext = appDbContext;
        }
        public async Task Handle(
            DeleteSubscriptionCommand request,
            CancellationToken cancellationToken)
        {
           var userId = _currentUserProvider.UserId;

            var subscription = await _dbContext.Subscriptions
                 .FirstOrDefaultAsync(
                s => s.Id == request.Id && s.UserId == userId,
                cancellationToken);

            if (subscription == null)
            {
                throw new NotFoundException(nameof(Subscription),request.Id);
            }

            _dbContext.Subscriptions.Remove(subscription);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
