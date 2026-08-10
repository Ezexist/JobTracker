using JobTracker.Application.Common.Models;
using MediatR;

namespace JobTracker.Application.Features.Subscriptions.GetById;

public sealed record GetSubscriptionByIdQuery(Guid Id) : IRequest<SubscriptionDto>;