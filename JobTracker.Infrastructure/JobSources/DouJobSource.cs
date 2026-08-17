using AngleSharp;
using JobTracker.Application.Common.Abstractions;
using JobTracker.Infrastructure.Dou;
using Microsoft.Extensions.Logging;

namespace JobTracker.Infrastructure.JobSources;

public sealed class DouJobSource : IJobSource
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DouJobSource> _logger;
    private readonly DouVacancyParser _parser;

    private const int PageSize = 20;
    private const int MaxPages = 5;
    private const int DelayBetweenPagesMs = 2000;

    public DouJobSource(
        HttpClient httpClient,
        ILogger<DouJobSource> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _parser = new DouVacancyParser();
    }

    public string SourceName => "DOU";

    public async Task<List<JobSourceVacancy>> FetchVacanciesAsync(CancellationToken cancellationToken)
    {
        var allVacancies = new List<JobSourceVacancy>();
        var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        try
        {
            for (var page = 0; page < MaxPages; page++)
            {
                var start = page * PageSize;
                var pageVacancies = await FetchPageAsync(start, cancellationToken);

                if (pageVacancies.Count == 0)
                {
                    _logger.LogInformation("No vacancies at start={Start}, stopping", start);
                    break;
                }

                var newOnPage = 0;
                foreach (var vacancy in pageVacancies)
                {
                    if (seenIds.Add(vacancy.ExternalId))
                    {
                        allVacancies.Add(vacancy);
                        newOnPage++;
                    }
                }

                _logger.LogInformation(
                    "DOU start={Start}: fetched {Count}, new {New}",
                    start, pageVacancies.Count, newOnPage);

                // Страница вернула только дубликаты → пагинация не работает, выходим
                if (newOnPage == 0)
                {
                    break;
                }

                if (page < MaxPages - 1)
                {
                    await Task.Delay(DelayBetweenPagesMs, cancellationToken);
                }
            }

            _logger.LogInformation("Total fetched {Count} vacancies from DOU", allVacancies.Count);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch vacancies from DOU");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while parsing DOU");
        }

        return allVacancies;
    }

    private async Task<List<JobSourceVacancy>> FetchPageAsync(int start, CancellationToken cancellationToken)
    {
        var vacancies = new List<JobSourceVacancy>();

        var url = $"/vacancies/?category=.NET&start={start}";
        var html = await _httpClient.GetStringAsync(url, cancellationToken);

        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html), cancellationToken);

        // Вакансии основного списка — <li class="l-vacancy">.
        // Боковая панель эту разметку не использует.
        var vacancyElements = document.QuerySelectorAll("li.l-vacancy");

        foreach (var element in vacancyElements)
        {
            try
            {
                var vacancy = _parser.ParseVacancy(element);
                if (vacancy is not null)
                {
                    vacancies.Add(vacancy);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse vacancy element at start={Start}", start);
            }
        }

        return vacancies;
    }
}