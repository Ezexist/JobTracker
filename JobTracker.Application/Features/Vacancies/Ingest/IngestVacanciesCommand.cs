using MediatR;


namespace JobTracker.Application.Features.Vacancies.Ingest
{
    public sealed record IngestVacanciesCommand(string? Source = null) : IRequest<int>;

}
