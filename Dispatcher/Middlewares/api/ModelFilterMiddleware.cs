using Dispatcher.FakeGpt;
using Dispatcher.Models.Openai;
using Newtonsoft.Json;

namespace Dispatcher.Middlewares.api;

public class ModelFilterMiddleware
{
    private readonly RequestDelegate _next;

    public ModelFilterMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        string requestBody;// 读取请求体
        using (var reader = new StreamReader(context.Request.Body))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        var requestModel = JsonConvert.DeserializeObject<EasyModel>(requestBody);
        var model = requestModel?.Model;
        if (model != null && model.Contains("gpt-4"))
        {
             var completion = Completion.GetDefaultOrExample($"当前请求的模型是{model},但是该模型消耗的金额较大。" +
                                                             $"为了保证长久使用，在该站点，该模型被禁用");
            await GptReply.Reply(context,completion);
            return;
        }

        context.Items["body"] = requestBody;
        await _next(context);
    }
}