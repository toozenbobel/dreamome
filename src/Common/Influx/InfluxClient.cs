using System.Collections.ObjectModel;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Options;

namespace Common.Influx;

public class InfluxClient : IInfluxClient
{
    private readonly InfluxSettings _settings;
    
    public InfluxClient(IOptions<InfluxSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task WriteMeasurement<T>(T measurement, string bucketName)
    {
        using var client = new InfluxDBClient(_settings.Host, _settings.Token);

        var writeApi = client.GetWriteApiAsync();
        await writeApi.WriteMeasurementAsync(measurement, WritePrecision.Ms, bucketName, _settings.OrgId);
    }

    public async Task WriteMeasurements<T>(IEnumerable<T> measurements, string bucketName)
    {
        using var client = new InfluxDBClient(_settings.Host, _settings.Token);

        var writeApi = client.GetWriteApiAsync();
        await writeApi.WriteMeasurementsAsync(measurements.ToList(), WritePrecision.Ms, bucketName, _settings.OrgId);
    }

    public async Task<ReadOnlyCollection<T>> Get<T>(string query)
    {
        using var client = new InfluxDBClient(_settings.Host, _settings.Token);

        var queryApi = client.GetQueryApi();
        var results = await queryApi.QueryAsync<T>(query, _settings.OrgId) ?? new List<T>();

        return results.AsReadOnly();
    }
}