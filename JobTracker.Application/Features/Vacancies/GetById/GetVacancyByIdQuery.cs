using JobTracker.Application.Common.Models;
using MediatR;

namespace JobTracker.Application.Features.Vacancies.GetById
{
    public sealed record GetVacancyByIdQuery(Guid Id) : IRequest<VacancyDto>;

}
