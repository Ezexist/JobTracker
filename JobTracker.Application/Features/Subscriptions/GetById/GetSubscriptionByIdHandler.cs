using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.Common.Models;
using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace JobTracker.Application.Features.Subscriptions.GetById
{
    public sealed class GetSubscriptionByIdHandler : IRequestHandler<GetSubscriptionByIdQuery, SubscriptionDto>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;
        public GetSubscriptionByIdHandler(IAppDbContext dbContext, ICurrentUserProvider currentUserProvider)
        {
            _dbContext = dbContext;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<SubscriptionDto> Handle(
            GetSubscriptionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserProvider.UserId;

            var subscription = await _dbContext.Subscriptions
                .AsNoTracking()
                .Where(s => s.Id == request.Id && s.UserId == userId)
                .Select(s => new SubscriptionDto
                (
                    s.Id,
                    s.Name,
                    s.IsActive,
                    s.RemoteOnly,
                    s.MinSalary,
                    s.Currency,
                    s.Keywords.Select(x => x.Value).ToList(),
                    s.Locations.Select(x => x.Value).ToList(),
                    s.CreatedAt
                ))
                .FirstOrDefaultAsync(cancellationToken);

            if (subscription == null)
            {
                throw new NotFoundException(nameof(Subscription), request.Id);
            }
            return subscription;
                
        }
    }
}
