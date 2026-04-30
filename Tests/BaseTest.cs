using Allure.NUnit;
using NUnit.Framework;
using RestTestsKozlenko.Client;
using RestTestsKozlenko.Helpers;

namespace RestTestsKozlenko.Tests;

/// <summary>
/// Base class for all test fixtures. Manages ApiClient lifetime and logging.
/// </summary>
[AllureNUnit]
public abstract class BaseTest
{
    protected ApiClient Client { get; private set; } = null!;
    protected TestLogger Logger { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Logger = new TestLogger();
        Client = new ApiClient(Logger);
        Logger.Log($"=== Test Started: {TestContext.CurrentContext.Test.Name} ===");
    }

    [TearDown]
    public void TearDown()
    {
        var result = TestContext.CurrentContext.Result.Outcome.Status;
        Logger.Log($"=== Test Finished: {TestContext.CurrentContext.Test.Name} | Status: {result} ===");
        Client.Dispose();
    }
}
