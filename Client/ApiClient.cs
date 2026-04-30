using System.Text;
using System.Text.Json;
using RestTestsKozlenko.Helpers;

namespace RestTestsKozlenko.Client;

public class ApiClient : IDisposable
{
    private readonly HttpClient _client;
    private readonly TestLogger? _logger;

    public ApiClient(TestLogger logger)
    {
        _logger = logger;
        var handler = new LoggingHandler(new HttpClientHandler());

        _client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
        };
    }

    public ApiClient(string baseUrl)
    {
        var handler = new LoggingHandler(new HttpClientHandler());

        _client = new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public async Task<HttpResponseMessage> Get(string endpoint)
        => await _client.GetAsync(endpoint);

    public async Task<HttpResponseMessage> Post(string endpoint, object body)
    {
        var json = JsonSerializer.Serialize(body);
        return await _client.PostAsync(endpoint,
            new StringContent(json, Encoding.UTF8, "application/json"));
    }

    public async Task<HttpResponseMessage> Put(string endpoint, object body)
    {
        var json = JsonSerializer.Serialize(body);
        return await _client.PutAsync(endpoint,
            new StringContent(json, Encoding.UTF8, "application/json"));
    }

    public async Task<HttpResponseMessage> Delete(string endpoint)
        => await _client.DeleteAsync(endpoint);

    public void Dispose()
    {
        _client?.Dispose();
    }
}