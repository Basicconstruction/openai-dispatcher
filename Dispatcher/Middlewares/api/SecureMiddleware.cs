using Dispatcher.Models;

namespace Dispatcher.Middlewares.api;


// 验证用户密钥的合法性，并尽早拦截用户请求
// 在此之前应该添加额外的不需要数据库的判断，比如ip过滤，内存数据对比
public class SecureMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _provider;

    public SecureMiddleware(RequestDelegate next,IServiceProvider provider)
    {
        _provider = provider;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var auth = context.Request.Headers.Authorization.ToString();
        if (auth.Length <= 7)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("错误的密钥，或者没有添加密钥");
            return;
        }
        var authKey = auth?[7..];
        using var scope = _provider.CreateScope();
        await using var data = scope.ServiceProvider.GetRequiredService<DataContext>();
        var openKey = data.OpenKeys.FirstOrDefault(key=>key.Key==authKey);
        if (openKey == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("使用的密钥不是我们签发的，或者在数据库中找不到这个密钥");
            return;
        }

        switch (openKey.PricingMethod)
        {
            case PricingMethod.RequestTime:
                if (openKey.AvailableRequest <= 0)
                {
                    context.Response.StatusCode = StatusCodes.Status423Locked;
                    await context.Response.WriteAsync("当前计价方式，请求次数已经耗尽");
                    return;
                }

                context.Items[nameof(PricingMethod)] = openKey.PricingMethod;
                break;
            case PricingMethod.Token:
                if (openKey.AvailableRequestToken <= 0)
                {
                    context.Response.StatusCode = StatusCodes.Status423Locked;
                    await context.Response.WriteAsync("当前计价方式，请求Token已经耗尽");
                    return;
                }
                context.Items[nameof(PricingMethod)] = openKey.PricingMethod;
                break;
            case null:
                context.Response.StatusCode = StatusCodes.Status423Locked;
                await context.Response.WriteAsync("请前往控制台选择当前的计价方式");
                return;
        }
        // 用户还有余额，并且选择了计价方式
        await _next(context);
    }
}