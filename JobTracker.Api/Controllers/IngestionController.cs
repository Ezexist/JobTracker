using JobTracker.Application.Features.Vacancies.Ingest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Api.Controllers
{
    [ApiController]
    [Route("api/ingestion")]
    public class IngestionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IngestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("run")]
        public async Task<ActionResult<int>> Run(
            [FromQuery]string? source,
            CancellationToken cancellationToken)
        {
            var command = new IngestVacanciesCommand(source);

            var processedCount = await _mediator.Send(command,cancellationToken);

            return Ok(new {processedCount});
        }
    }
}
