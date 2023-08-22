namespace Dispatcher.Middlewares.api;

public class PricingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _provider;
    public PricingMiddleware(RequestDelegate next,IServiceProvider provider)
    {
        _next = next;
        _provider = provider;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        await _next(context);
        await context.Response.WriteAsync((context.Items["PickedKey"] ?? "").ToString() ?? string.Empty);
        //await context.Response.WriteAsync("appended");
        // TODO: 在此处添加您的代码
    }
}