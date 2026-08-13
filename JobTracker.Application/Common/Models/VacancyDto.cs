namespace JobTracker.Application.Common.Models;

public sealed record VacancyDto(
    Guid Id,
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
    DateTimeOffset? PublishedAt,
    DateTimeOffset DetectedAt);