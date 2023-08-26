using Dispatcher.Models.Openai;
using Newtonsoft.Json;

namespace Dispatcher.FakeGpt;

public class GptReplyer
{
    public static async Task Reply(HttpContext context,Completion completion)
    {
        await context.Response.WriteAsync($"event: message\n");

        await context.Response.WriteAsync($"data: {JsonConvert.SerializeObject(completion)}\n\n");

        await context.Response.Body.FlushAsync();

        await context.Response.WriteAsync("[done]");
    }
}