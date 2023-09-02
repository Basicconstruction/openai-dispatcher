using Dispatcher.Models.Openai;
using Newtonsoft.Json;

namespace Dispatcher.FakeGpt;

public abstract class GptReply
{
    public static async Task Reply(HttpContext context,Completion completion)
    {
        context.Response.Headers.Add("Content-Type", "text/event-stream");
        // await context.Response.WriteAsync($"event: message\n");
        await context.Response.WriteAsync($"data: {JsonConvert.SerializeObject(completion)}\n\n");
        await context.Response.Body.FlushAsync();
        await context.Response.WriteAsync("[done]");
    }

    public static async Task ReplyStart(HttpContext context, Completion completion)
    {
        context.Response.Headers.Add("Content-Type", "text/event-stream");
        // await context.Response.WriteAsync($"event: message\n");
        await context.Response.WriteAsync($"data: {JsonConvert.SerializeObject(completion)}\n\n");
    }
    public static async Task ReplyStart(HttpContext context, string result)
    {
        var completion = Completion.GetDefaultOrExample(result);
        await ReplyStart(context, completion);
    }

    public static async Task ReplyJust(HttpContext context, Completion completion)
    {
        // await context.Response.WriteAsync($"event: message\n");
        await context.Response.WriteAsync($"data: {JsonConvert.SerializeObject(completion)}\n\n");
    }

    public static async Task ReplyJust(HttpContext context, string result)
    {
        var completion = Completion.GetDefaultOrExample(result);
        await ReplyJust(context, completion);
    }
    public static async Task ReplyEnd(HttpContext context)
    {
        await context.Response.WriteAsync("[done]");
        await context.Response.Body.FlushAsync();
    }

}