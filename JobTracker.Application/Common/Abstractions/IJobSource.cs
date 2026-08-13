

namespace JobTracker.Application.Common.Abstractions
{
    public interface IJobSource
    {
        string SourceName { get; }

        Task<List<JobSourceVacancy>> FetchVacanciesAsync(CancellationToken cancellationToken);
    }
}
