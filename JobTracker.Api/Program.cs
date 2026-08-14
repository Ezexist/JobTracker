using JobTracker.Api.Middleware;
using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Common.Extensions;
using JobTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

using JobTracker.Infrastructure.JobSources;
using JobTracker.Application.Common.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());


builder.Services.AddHttpClient<DouJobSource>(client =>
{
    client.BaseAddress = new Uri("https://jobs.dou.ua");
    client.DefaultRequestHeaders.Add(
        "User-Agent",
        "JobTracker/1.0");
    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml");
    client.DefaultRequestHeaders.Add("Accept-Language", "uk-UA,uk;q=0.9,en-US;q=0.8,en;q=0.7");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped<IJobSource, DouJobSource>(provider =>
    provider.GetRequiredService<DouJobSource>());

builder.Services.AddAplication();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");


await DatabaseSeeder.SeedDefaultUserAsync(app.Services);

app.Run();

public partial class Program { }

