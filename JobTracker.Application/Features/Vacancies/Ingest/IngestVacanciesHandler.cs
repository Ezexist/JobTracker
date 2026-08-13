using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Features.Vacancies.Create;
using MediatR;

namespace JobTracker.Application.Features.Vacancies.Ingest
{
    public sealed class IngestVacanciesHandler : IRequestHandler<IngestVacanciesCommand, int>
    {
        private readonly IEnumerable<IJobSource> _sources;
        private readonly IMediator _mediator;
        public IngestVacanciesHandler(IEnumerable<IJobSource> sources, IMediator mediator)
        {
            _sources = sources;
            _mediator = mediator;
        }
        public async Task<int> Handle(
            IngestVacanciesCommand request,
            CancellationToken cancellationToken)
        {
            var sourcesToProcess = string.IsNullOrEmpty(request.Source)
                ? _sources.ToList()
                : _sources.Where(s => s.SourceName == request.Source).ToList();

            var totalProcessed = 0;

            foreach (var source in sourcesToProcess)
            {
                var vacancies = await source.FetchVacanciesAsync(cancellationToken);

                foreach(var vacancy in vacancies)
                {
                    var command = new CreateVacancyCommand(
                        Source: source.SourceName,
                        ExternalId: vacancy.ExternalId,
                        Title: vacancy.Title,
                        Company: vacancy.Company,
                        Location: vacancy.Location,
                        IsRemote: vacancy.IsRemote,
                        SalaryMin: vacancy.SalaryMin,
                        SalaryMax: vacancy.SalaryMax,
                        Currency: vacancy.Currency,
                        Url: vacancy.Url,
                        Description: vacancy.Description,
                        PublishedAt: vacancy.PublishedAt);

                    await _mediator.Send(command,cancellationToken);

                    totalProcessed++;
                }
            }

            return totalProcessed;
        }
    }
}
