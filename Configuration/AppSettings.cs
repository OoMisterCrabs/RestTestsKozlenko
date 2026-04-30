using Microsoft.Extensions.Configuration;

namespace RestTestsKozlenko.Configuration;

public static class AppSettings
{
    private static readonly IConfiguration _configuration;

    static AppSettings()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static string BaseUrl => _configuration["BaseUrl"]
        ?? throw new InvalidOperationException("BaseUrl is not configured in appsettings.json");
}
