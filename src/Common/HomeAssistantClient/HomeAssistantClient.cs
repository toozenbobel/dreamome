using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HomeAssistantClient;

public class HomeAssistantClient : IHomeAssistantClient
{
    private readonly HttpClient _client;

    public HomeAssistantClient(HttpClient client)
    {
        _client = client;
    }

    public async Task CallService<T>(string domain, string serviceName, T data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        });
        
        var url = string.Format(HomeAssistantApi.Services, domain, serviceName);
        var result = await _client.PostAsync(url, new StringContent(json, Encoding.Default, MediaTypeNames.Application.Json));

        await result.Content.ReadAsStringAsync();
        
        result.EnsureSuccessStatusCode();
    }
}