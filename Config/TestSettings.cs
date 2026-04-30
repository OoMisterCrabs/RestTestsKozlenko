using Microsoft.Extensions.Configuration;

public class TestSettings
{
    public string BaseUrl { get; set; } = "https://jsonplaceholder.typicode.com";

    public static TestSettings Load()
    {
        try
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var baseUrl = config["TestSettings:BaseUrl"];
            return new TestSettings { BaseUrl = baseUrl ?? "https://jsonplaceholder.typicode.com" };
        }
        catch
        {
            return new TestSettings { BaseUrl = "https://jsonplaceholder.typicode.com" };
        }
    }
}