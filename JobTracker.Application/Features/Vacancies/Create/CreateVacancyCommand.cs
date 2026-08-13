using MediatR;

namespace JobTracker.Application.Features.Vacancies.Create
{
    public sealed record CreateVacancyCommand(
        string Source,
        string ExternalId,
        string Title,
        string? Company,
        string? Location,
        bool IsRemote,
        int? SalaryMin,
        int? SalaryMax,
        string? Currency,
        string Url,
        string? Description,
        DateTimeOffset? PublishedAt) : IRequest<Guid>
    {
    }
}
