using Common.Influx;
using MediatR;
using WeatherAlerts.Entities;
using WeatherAlerts.Settings;

namespace WeatherAlerts.Commands;

internal record DumpMeteoWarningsCommand(IReadOnlyCollection<MeteoWarningEntity> Warnings) : IRequest
{
    public class DumpMeteoWarningsCommandHandler : IRequestHandler<DumpMeteoWarningsCommand>
    {
        private readonly IInfluxClient _influxClient;

        public DumpMeteoWarningsCommandHandler(IInfluxClient influxClient)
        {
            _influxClient = influxClient;
        }
        
        public async Task Handle(DumpMeteoWarningsCommand request, CancellationToken cancellationToken)
        {
            await _influxClient.WriteMeasurements(request.Warnings, InfluxBuckets.MainBucketName);
        }
    }
}