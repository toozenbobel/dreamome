using InfluxDB.Client.Api.Domain;

namespace Common.Influx;

public interface IInfluxManagementClient
{
    Task EnsureBucketCreated(string bucketName, BucketRetentionRules retentionRules);
}