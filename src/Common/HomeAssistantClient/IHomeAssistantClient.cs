namespace HomeAssistantClient;

public interface IHomeAssistantClient
{
    Task CallService<T>(string domain, string serviceName, T data);
}