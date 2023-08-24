using Dispatcher.Models;

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
        var pickedKey = (OpenKey)context.Items["RequestKey"];
        using var scope = _provider.CreateScope();
        await using var data = scope.ServiceProvider.GetRequiredService<DataContext>();
        if (pickedKey == null)
        {
            return;
        }

        var key = data.OpenKeys.FirstOrDefault(key => key.OpenKeyId == pickedKey.OpenKeyId);
        if (key != null)
        {
            key.AvailableRequest -= 1;
            await data.SaveChangesAsync();
        }
        //await context.Response.WriteAsync((context.Items["PickedKey"] ?? "").ToString() ?? string.Empty);
        //await context.Response.WriteAsync("appended");

    }
}