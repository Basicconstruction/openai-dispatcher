using Dispatcher.FakeGpt;
using Dispatcher.Models;
using Dispatcher.Models.Openai;
using Dispatcher.Models.Requests;

namespace Dispatcher.Middlewares.api;

// 根据 一定的策略从动态密钥池中选取一个密钥和请求地址
public class RequestChooserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly KeyPoolRepository _repository;
    public RequestChooserMiddleware(RequestDelegate next,KeyPoolRepository repository)
    {
        _next = next;
        _repository = repository;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var random = new Random();
        if (_repository.Count < 1)
        {
            var completion = Completion.GetDefaultOrExample("当前动态池中没有实际请求密钥，请重试。（程序错误）");
            await GptReply.Reply(context,completion);
            return;
        }
        var index = random.Next(_repository.Count);
        var pickedKey = _repository.PoolKeys?[index];
        if (pickedKey?.Available == false)
        {
            var completion = Completion.GetDefaultOrExample("数据异常，请联系管理员！");
            await GptReply.Reply(context,completion);
            return;
        }
        context.Items["PickedKey"] = pickedKey;
        await _next(context);
    }
}