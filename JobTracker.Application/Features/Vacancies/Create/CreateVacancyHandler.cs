using JobTracker.Application.Common.Abstractions;
using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace JobTracker.Application.Features.Vacancies.Create
{
    public sealed class CreateVacancyHandler : IRequestHandler<CreateVacancyCommand, Guid>
    {
        private readonly IAppDbContext _dbContext;
        public CreateVacancyHandler(IAppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task<Guid> Handle(
            CreateVacancyCommand request, 
            CancellationToken cancellationToken)
        {
            var existingVacancy = await _dbContext.Vacancies
                 .AsNoTracking()
                 .FirstOrDefaultAsync(
                v => v.Source == request.Source && v.ExternalId == request.ExternalId,
                cancellationToken);

            if(existingVacancy is not null)
            {
                return existingVacancy.Id;
            }

            var vacany = new Vacancy
            {
                Source = request.Source,
                ExternalId = request.ExternalId,
                Title = request.Title,
                Company = request.Company,
                Location = request.Location,
                IsRemote = request.IsRemote,
                SalaryMin = request.SalaryMin,
                SalaryMax = request.SalaryMax,
                Currency = request.Currency,
                Url = request.Url,
                Description = request.Description,
                PublishedAt = request.PublishedAt,
                DetectedAt = DateTimeOffset.UtcNow
            };

            _dbContext.Vacancies.Add(vacany);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return vacany.Id;
        }
    }
}
