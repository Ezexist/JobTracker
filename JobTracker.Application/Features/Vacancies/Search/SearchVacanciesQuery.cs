using JobTracker.Application.Common.Models;
using MediatR;

namespace JobTracker.Application.Features.Vacancies.Search
{
    public sealed record SearchVacanciesQuery(
        string? Keyword,
        string? Location,
        bool? IsRemote,
        int? MinSalary,
        int Page = 1,
        int PageSize = 20) : IRequest<SearchResult<VacancyDto>>;

}
