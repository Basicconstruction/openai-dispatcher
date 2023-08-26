using Dispatcher.Models;

namespace Dispatcher.Middlewares.api;

public class TestMiddleware
{
    private RequestDelegate _next;

    public TestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context,DataContext dataContext)
    {
        await context.Response.WriteAsync("test "+dataContext.GetHashCode().ToString());
        await _next(context);

        // TODO: 在此处添加您的代码
    }
}