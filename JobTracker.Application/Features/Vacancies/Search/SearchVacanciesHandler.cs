using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Vacancies.Search
{
    public sealed class SearchVacanciesHandler : IRequestHandler<SearchVacanciesQuery, SearchResult<VacancyDto>>
    {
        private readonly IAppDbContext _dbContext;
        public SearchVacanciesHandler(IAppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task<SearchResult<VacancyDto>> Handle(
            SearchVacanciesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Vacancies
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var keyword = request.Keyword.Trim();

                query = query.Where(v =>
                v.Title.Contains(keyword) ||
                (v.Description != null && v.Description.Contains(keyword)));
            }

            if (!string.IsNullOrWhiteSpace(request.Location))
            {
                var location = request.Location.Trim();

                query = query.Where(v =>
                v.Location != null && v.Location.Contains(location));
            }

            if (request.IsRemote.HasValue)
            {
                query = query.Where(v => v.IsRemote == request.IsRemote.Value);
            }

            if (request.MinSalary.HasValue)
            {
                query = query.Where(v => v.SalaryMax.HasValue && v.SalaryMax >= request.MinSalary.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(v => v.DetectedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
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
                .ToListAsync(cancellationToken);

            return new SearchResult<VacancyDto>(
                items,
                totalCount,
                request.Page,
                request.PageSize);
                
        }
    }
}
