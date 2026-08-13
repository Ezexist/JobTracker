using JobTracker.Application.Common.Models;
using JobTracker.Application.Features.Vacancies.Create;
using JobTracker.Application.Features.Vacancies.GetById;
using JobTracker.Application.Features.Vacancies.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Services;

namespace JobTracker.Api.Controllers
{
    [ApiController]
    [Route("api/vacancies")]
    public class VacanciesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VacanciesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromBody] CreateVacancyCommand command,
            CancellationToken cancellationToken)
        {
            var vacancyId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(Create), vacancyId);
        }

        [HttpGet("search")]
        public async  Task<ActionResult<SearchResult<VacancyDto>>> Search(
            [FromQuery] SearchVacanciesQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VacancyDto>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetVacancyByIdQuery(id);

            var vacancy = await _mediator.Send(query, cancellationToken);

            return Ok(vacancy);
        }
    }
}
