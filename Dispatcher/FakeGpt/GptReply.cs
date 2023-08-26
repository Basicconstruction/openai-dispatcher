using Dispatcher.Models.Openai;
using Newtonsoft.Json;

namespace Dispatcher.FakeGpt;

public abstract class GptReply
{
    public static async Task Reply(HttpContext context,Completion completion)
    {
        context.Response.Headers.Add("Content-Type", "text/event-stream");
        await context.Response.WriteAsync($"event: message\n");
        await context.Response.WriteAsync($"data: {JsonConvert.SerializeObject(completion)}\n\n");
        await context.Response.Body.FlushAsync();
        await context.Response.WriteAsync("[done]");
    }
}