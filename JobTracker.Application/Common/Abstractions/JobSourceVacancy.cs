namespace JobTracker.Application.Common.Abstractions;

public sealed record JobSourceVacancy(
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
    DateTimeOffset? PublishedAt);