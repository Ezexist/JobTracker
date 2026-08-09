using System.Reflection;
using FluentValidation;
using JobTracker.Application.Common.Abstractions;
using JobTracker.Application.Common.Behaviors;
using JobTracker.Application.Common.CurrentUser;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
