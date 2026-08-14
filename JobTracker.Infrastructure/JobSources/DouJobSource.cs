using AngleSharp;
using AngleSharp.Dom;
using JobTracker.Application.Common.Abstractions;
using JobTracker.Infrastructure.Dou;
using Microsoft.Extensions.Logging;

namespace JobTracker.Infrastructure.JobSources;

public sealed class DouJobSource : IJobSource
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DouJobSource> _logger;
    private readonly DouVacancyParser _parser;

    private const int MaxPages = 10;
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

        try
        {
            for (int page = 1; page <= MaxPages; page++)
            {
                var vacancies = await FetchPageAsync(page, cancellationToken);

                if (vacancies.Count == 0)
                {
                    _logger.LogInformation("No more vacancies found on page {Page}", page);
                    break;
                }

                allVacancies.AddRange(vacancies);
                _logger.LogInformation("Fetched {Count} vacancies from page {Page}", vacancies.Count, page);

                // Задержка между страницами для этичности
                if (page < MaxPages)
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

    private async Task<List<JobSourceVacancy>> FetchPageAsync(int page, CancellationToken cancellationToken)
    {
        var vacancies = new List<JobSourceVacancy>();

        var url = $"/vacancies/?category=.NET&page={page}";
        var html = await _httpClient.GetStringAsync(url, cancellationToken);

        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html), cancellationToken);

        // Ищем контейнер со списком вакансий (не боковую панель)
        var vacancyListContainer = document.QuerySelector("div.vt-list")
                                ?? document.QuerySelector("div#vacancyList");

        if (vacancyListContainer is null)
        {
            _logger.LogWarning("Could not find vacancy list container on page {Page}", page);
            return vacancies;
        }

        // Ищем элементы вакансий внутри контейнера
        var vacancyElements = vacancyListContainer.QuerySelectorAll("div.vacancy");

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
                _logger.LogWarning(ex, "Failed to parse vacancy element on page {Page}", page);
            }
        }

        return vacancies;
    }
}