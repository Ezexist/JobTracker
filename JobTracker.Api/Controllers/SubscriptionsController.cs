using JobTracker.Application.Features.Subscriptions.Create;
using JobTracker.Application.Features.Subscriptions.Delete;
using JobTracker.Application.Features.Subscriptions.List;
using JobTracker.Application.Features.Subscriptions.Toggle;
using JobTracker.Application.Features.Subscriptions.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using JobTracker.Application.Common.Models;
using JobTracker.Application.Features.Subscriptions.GetById;

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

        [HttpGet]
        public async Task<ActionResult<List<SubscriptionDto>>> GetList(CancellationToken cancellationToken)
        {
            var query = new GetSubscriptionListQuery();
            var subscriptions = await _mediator.Send(query, cancellationToken);

            return Ok(subscriptions);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            [FromBody] UpdateSubscriptionCommand command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id,CancellationToken cancellationToken)
        {
            var command = new DeleteSubscriptionCommand(id);

            await _mediator.Send(command,cancellationToken);

            return NoContent();
        }
        [HttpPatch("{id}/toggle")]
        public async Task<ActionResult<bool>> Toggle(
            Guid id,
            CancellationToken cancellationToken)
        {
           var command = new ToggleSubscriptionCommand(id);

           var isActive =  await _mediator.Send(command, cancellationToken);

            return Ok(isActive);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetSubscriptionByIdQuery(id);

            var subcription = await _mediator.Send(query,cancellationToken);

            return Ok(subcription);
        }
    }
}
