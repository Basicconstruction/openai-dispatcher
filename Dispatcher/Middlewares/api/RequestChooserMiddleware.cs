using Dispatcher.Models;
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
        var pickedKey = _repository.PoolKeys[random.Next(_repository.Count)];
        context.Items["PickedKey"] = pickedKey;
        await _next(context);
    }
}