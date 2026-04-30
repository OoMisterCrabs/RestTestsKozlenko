using NUnit.Framework;
namespace RestTestsKozlenko.Helpers;

/// <summary>
/// Simple logger that writes to NUnit TestContext output.
/// </summary>
public class TestLogger
{
    public void Log(string message)
    {
        var line = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
        TestContext.WriteLine(line);
        Console.WriteLine(line);
    }
}
