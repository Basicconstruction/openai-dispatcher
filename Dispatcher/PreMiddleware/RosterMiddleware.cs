using System.Text;
using Dispatcher.Models;

namespace Dispatcher.PreMiddleware;

// 这个包需要处理请求拦截，过滤ip等减少无用流量
public class RosterMiddleware
{
    private DynamicTable _table;
    private readonly RequestDelegate _next;

    public RosterMiddleware(DynamicTable table,RequestDelegate next)
    {
        _table = table;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var headers = context.Request.Headers;
        //await context.Response.WriteAsync($"{context.Request.Headers.Detail()}");
        string ip = "";
        if (!headers.ContainsKey("ip"))
        {
            try
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                // ignored
            }
        }
        else
        {
            ip = headers["ip"];
        }

        var auth = headers.Authorization.ToString()??"";
        string key = "";
        if (auth.Length <= 7)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("没有输入密钥，请输入密钥。");
            return;
        }

        key = auth[7..];

        if (!_table.Accept(key, ip))
        {
            context.Response.StatusCode = StatusCodes.Status102Processing;
            await context.Response.WriteAsync("请求速度过快，请等待一分钟");
            return;
        }

        await _next(context);
    }
}
public static class HeaderDetails{
    public static string Detail(this IHeaderDictionary dictionary)
    {
        var sb = new StringBuilder();
        foreach (var pair in dictionary)
        {
            sb.Append($"{pair.Key}: {pair.Value}\n");
        }

        return sb.ToString();
    }
}