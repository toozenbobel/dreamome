using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Settings.Configuration;

namespace Common.Extensions;

public static class HostBuilderExtensions
{
    public static void AddSecrets(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddKeyPerFile("/run/secrets", true);
            config.AddUserSecrets(Assembly.GetExecutingAssembly());
        });
    }

    public static void AddSerilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((ctx, configuration) =>
        {
            configuration.ReadFrom.Configuration(ctx.Configuration, new ConfigurationReaderOptions
            {
                SectionName = "Serilog:System"
            });
        });

        hostBuilder.ConfigureServices(s => s.AddSingleton(Log.Logger));
    }

    public static void AddSwagger(this WebApplicationBuilder builder, string apiTitle)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = apiTitle,
                Version = "v1.0"
            });
        });
    }
}