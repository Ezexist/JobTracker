using FluentValidation;
using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Common.Behaviors;
using JobTracker.Application.Common.CurrentUser;
using JobTracker.Application.Features.JobSources;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JobTracker.Application.Common.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<ICurrentUserProvider,SingleUserProvider>();
            services.AddScoped<IJobSource, FakeJobSource>();

            return services;
        }
    }
}
