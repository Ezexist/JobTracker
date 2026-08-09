using JobTracker.Application.Features.Subscriptions.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Api.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateSubscriptionCommand command,
            CancellationToken cancellationToken)
        {
            var subscriptionId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(Create), subscriptionId);
        }
    }
}
