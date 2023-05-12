using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Options;
using Serilog;

namespace Common.Influx;

public class InfluxManagementClient : IInfluxManagementClient
{
    private readonly InfluxSettings _settings;

    public InfluxManagementClient(IOptions<InfluxSettings> settings)
    {
        _settings = settings.Value;
    }
    
    public async Task EnsureBucketCreated(string bucketName, BucketRetentionRules retentionRules)
    {
        using var client = new InfluxDBClient(_settings.Host, _settings.Token);

        var bucketsApi = client.GetBucketsApi();

        var bucket = await bucketsApi.FindBucketByNameAsync(bucketName);
        if (bucket is not null)
        {
            Log.Information("Bucket {BucketName} exists", bucketName);
            return;
        }

        await bucketsApi.CreateBucketAsync(bucketName, retentionRules, _settings.OrgId);
        Log.Information("Bucket {BucketName} created", bucketName);
    }
}