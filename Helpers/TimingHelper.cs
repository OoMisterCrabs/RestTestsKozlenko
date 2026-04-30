using System.Diagnostics;

public static class TimingHelper
{
    public static async Task<(HttpResponseMessage, long)> Measure(Func<Task<HttpResponseMessage>> action)
    {
        var sw = Stopwatch.StartNew();
        var response = await action();
        sw.Stop();

        return (response, sw.ElapsedMilliseconds);
    }
}