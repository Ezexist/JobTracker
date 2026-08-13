using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.Common.Models;
using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace JobTracker.Application.Features.Vacancies.GetById
{
    public sealed class GetVacancyByIdHandler : IRequestHandler<GetVacancyByIdQuery, VacancyDto>
    {
        private readonly IAppDbContext _dbContext;
        public GetVacancyByIdHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<VacancyDto> Handle(
            GetVacancyByIdQuery request,
            CancellationToken cancellationToken)
        {
            var vacancy = await _dbContext.Vacancies
                .AsNoTracking()
                .Where(v => v.Id == request.Id)
                .Select(v => new VacancyDto(
                    v.Id,
                    v.Source,
                    v.ExternalId,
                    v.Title,
                    v.Company,
                    v.Location,
                    v.IsRemote,
                    v.SalaryMin,
                    v.SalaryMax,
                    v.Currency,
                    v.Url,
                    v.Description,
                    v.PublishedAt,
                    v.DetectedAt))
                .FirstOrDefaultAsync(cancellationToken);

            if(vacancy == null)
            {
                throw new NotFoundException(nameof(Vacancy), request.Id);
            }

            return vacancy;
        }
    }
}
