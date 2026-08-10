using MediatR;

namespace JobTracker.Application.Features.Subscriptions.Toggle;

public sealed record ToggleSubscriptionCommand(Guid Id) : IRequest<bool>;