using System.Collections.ObjectModel;

namespace Common.Influx;

public interface IInfluxClient
{
    Task WriteMeasurement<T>(T measurement, string bucketName);
    Task<ReadOnlyCollection<T>> Get<T>(string query);
    Task WriteMeasurements<T>(IEnumerable<T> measurements, string bucketName);
}