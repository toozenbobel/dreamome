using System.Text.Json.Serialization;
using Common.Filters;
using Common.PipelineBehaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions;

public static class ServicesExtensions
{
    public static void AddFilters(this IServiceCollection services)
    {
        services.AddControllers(config =>
            {
                config.Filters.Add(typeof(GlobalExceptionFilter));
                // todo: add transaction filter
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.WriteIndented = true;
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
    }

    public static void AddMediatrBehaviors(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}