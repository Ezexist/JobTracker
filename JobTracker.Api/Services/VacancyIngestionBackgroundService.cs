using JobTracker.Application.Features.Vacancies.Ingest;
using MediatR;

namespace JobTracker.Api.Services
{
    public sealed class VacancyIngestionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VacancyIngestionBackgroundService> _logger;

        private static readonly TimeSpan IngestionInterval = TimeSpan.FromMinutes(30);

        public VacancyIngestionBackgroundService(IServiceProvider serviceProvider, 
            ILogger<VacancyIngestionBackgroundService> logger) 
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Vacancy Ingestion Background Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await IngestVacanciesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during vacancy ingestion");
                }
                await Task.Delay(IngestionInterval, stoppingToken);
            }

            _logger.LogInformation("Vacancy Ingestion Background serivce stopped");
        }

        private async Task IngestVacanciesAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting vacancy ingestion at {tTme}", DateTimeOffset.UtcNow);

            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var command = new IngestVacanciesCommand();
            var processedCount = await mediator.Send(command, stoppingToken);

            _logger.LogInformation("Vacancy ingestion completed. Processed {Count} vacancies", processedCount);
        }
    }
}
