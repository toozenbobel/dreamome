using Common.Influx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions;

public static class InfluxExtensions
{
    public static void AddInflux(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.Configure<InfluxSettings>(configurationManager.GetSection("Influx"));
        services.AddScoped<IInfluxManagementClient, InfluxManagementClient>();
        services.AddScoped<IInfluxClient, InfluxClient>();
    }
}