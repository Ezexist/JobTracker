using JobTracker.Application.Common.Abstractions;

namespace JobTracker.Application.Features.JobSources
{
    public sealed class FakeJobSource : IJobSource
    {
        public string SourceName => "Fake source";

        public Task<List<JobSourceVacancy>> FetchVacanciesAsync(CancellationToken cancellationToken)
        {
            var vacancies = new List<JobSourceVacancy> {
              new(
                ExternalId: "fake-001",
                Title: "Senior .NET Developer",
                Company: "TechCorp",
                Location: "Kyiv",
                IsRemote: false,
                SalaryMin: 3000,
                SalaryMax: 5000,
                Currency: "USD",
                Url: "https://fake.jobs/vacancy/001",
                Description: "We are looking for a Senior .NET Developer with 5+ years of experience",
                PublishedAt: DateTimeOffset.UtcNow.AddDays(-1)),

            new(
                ExternalId: "fake-002",
                Title: "C# Developer (Remote)",
                Company: "RemoteFirst",
                Location: "Remote",
                IsRemote: true,
                SalaryMin: 2500,
                SalaryMax: 4000,
                Currency: "USD",
                Url: "https://fake.jobs/vacancy/002",
                Description: "Fully remote position for C# developer",
                PublishedAt: DateTimeOffset.UtcNow.AddDays(-2)),

            new(
                ExternalId: "fake-003",
                Title: "ASP.NET Core Engineer",
                Company: "WebSolutions",
                Location: "Lviv",
                IsRemote: false,
                SalaryMin: 2000,
                SalaryMax: 3500,
                Currency: "USD",
                Url: "https://fake.jobs/vacancy/003",
                Description: "Building modern web applications with ASP.NET Core",
                PublishedAt: DateTimeOffset.UtcNow.AddDays(-3)),

            new(
                ExternalId: "fake-004",
                Title: "Azure Developer",
                Company: "CloudTech",
                Location: "Remote",
                IsRemote: true,
                SalaryMin: 3500,
                SalaryMax: 5500,
                Currency: "USD",
                Url: "https://fake.jobs/vacancy/004",
                Description: "Working with Azure cloud services and microservices",
                PublishedAt: DateTimeOffset.UtcNow),

            new(
                ExternalId: "fake-005",
                Title: "Junior .NET Developer",
                Company: "StartUp Inc",
                Location: "Kyiv",
                IsRemote: false,
                SalaryMin: 1000,
                SalaryMax: 1500,
                Currency: "USD",
                Url: "https://fake.jobs/vacancy/005",
                Description: "Great opportunity for junior developers to grow",
                PublishedAt: DateTimeOffset.UtcNow.AddDays(-5))
            };

            return Task.FromResult(vacancies);
                
        }
    }
}
