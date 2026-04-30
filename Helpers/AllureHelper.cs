using Allure.Net.Commons;
using Allure.NUnit;

public static class AllureHelper
{
    public static async Task AttachResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        AllureApi.AddAttachment(
            "Response",
            "application/json",
            content
        );
    }
}