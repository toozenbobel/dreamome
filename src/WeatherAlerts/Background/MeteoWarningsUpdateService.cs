using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Serilog;
using WeatherAlerts.Services;
using WeatherAlerts.Settings;

namespace WeatherAlerts.Background;

internal class MeteoWarningsUpdateService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodicTimer _timer;

    public MeteoWarningsUpdateService(IServiceProvider serviceProvider, IOptions<AlertsSettings> settings)
    {
        _serviceProvider = serviceProvider;
        _timer = new PeriodicTimer(settings.Value.UpdateInterval);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("{Service} has started", nameof(MeteoWarningsUpdateService));

        do
        {
            await DoProcessing();
        } while (await _timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task DoProcessing()
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                Log.Information("Started updating weather alerts");

                var dumper = scope.ServiceProvider.GetRequiredService<IMeteoWarningsDumper>();
                await dumper.LoadAndDump();

                Log.Information("Finished processing weather alerts");
            }
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to updated weather alerts");
        }
    }
    
    [ExcludeFromCodeCoverage]
    public override void Dispose()
    {
        _timer.Dispose();
    }
}