using Dispatcher.Models;

namespace Dispatcher.Middlewares.api;

public class PricingMiddleware
{
    private readonly RequestDelegate _next;
    public PricingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context,DataContext data)
    {
        await _next(context);
        if ((bool)(context.Items["Free"] ?? false))
        {
            // 不计费
        }
        else
        {
            var pickedKey = (OpenKey)context.Items["RequestKey"];
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
        }
    }
}