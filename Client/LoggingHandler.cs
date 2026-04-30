using System.Net.Http;
using NUnit.Framework;

namespace RestTestsKozlenko.Client;

public class LoggingHandler : DelegatingHandler
{
    public LoggingHandler(HttpMessageHandler inner) : base(inner) { }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
    {
        TestContext.WriteLine($"REQUEST: {request.Method} {request.RequestUri}");

        var response = await base.SendAsync(request, token);

        var body = await response.Content.ReadAsStringAsync();
        TestContext.WriteLine($"RESPONSE: {response.StatusCode}");
        TestContext.WriteLine(body);

        return response;
    }
}