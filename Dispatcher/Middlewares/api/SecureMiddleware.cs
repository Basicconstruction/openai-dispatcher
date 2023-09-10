using System.Text.RegularExpressions;
using Dispatcher.Endpoints;
using Dispatcher.FakeGpt;
using Dispatcher.Models;
using Dispatcher.Models.Requests;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;

namespace Dispatcher.Middlewares.api;


// 验证用户密钥的合法性，并尽早拦截用户请求
// 在此之前应该添加额外的不需要数据库的判断，比如ip过滤，内存数据对比
public class SecureMiddleware
{
    private DynamicTable _table;
    private readonly RequestDelegate _next;
    private readonly RunConfiguration _configuration;

    public SecureMiddleware(RequestDelegate next,DynamicTable table,IOptions<RunConfiguration> confs)
    {
        _configuration = confs.Value;
        _next = next;
        _table = table;
    }

    public async Task Invoke(HttpContext context, DataContext data)
    {
        var auth = context.Request.Headers.Authorization.ToString();
        if (auth.Length <= 7)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("错误的密钥，或者没有添加密钥");
            return;
        }
        var authKey = auth?[7..];
        async Task KeyNotAllow(string? msg = null)
        {
            await new TestTransferEndpoint().Endpoint(context);// 如果不是我们签发的，直接返回fake gpt
            msg ??= "使用的密钥不是我们签发的，或者在数据库中找不到这个密钥";
            await context.Response.WriteAsync(msg);
        }

        OpenKey? openKey;
        if (_table.IsNotAllowKey(authKey ?? ""))
        {
            await KeyNotAllow();
            return;
        }

        if (_configuration.OpenForPublic)
        {
            var lowerKey = authKey.ToLower();
            if (lowerKey.StartsWith("c"))
            {
                // 使用chatanywhere 转发
                context.Items["RequestKey"] = authKey[1..];
                context.Items["Free"] = true;
                context.Items["ApiUrl"] = "https://api.chatanywhere.com.cn";
            }else if (lowerKey.StartsWith("ss"))
            {
                // 使用sb转发
                context.Items["RequestKey"] = authKey[1..];
                context.Items["Free"] = true;
                context.Items["ApiUrl"] = "https://api.openai-sb.com";
            }else if (lowerKey.StartsWith("o"))
            {
                // 使用openai，用alias转发
                context.Items["RequestKey"] = authKey[1..];
                context.Items["Free"] = true;
                context.Items["ApiUrl"] = "https://aliasbot.asia";
            }
            else if (lowerKey.StartsWith("h"))
            {

                var list = authKey.Trim().Split(" ");
                // 手动填写地址转发
                context.Items["Free"] = true;
                context.Items["ApiUrl"] = list[0];
                context.Items["RequestKey"] = list[1];
            }
        }

        if (!(bool)(context.Items["Free"] ?? false))
        {
            openKey = data.OpenKeys.FirstOrDefault(key => key.Key == authKey);
            if (openKey == null)
            {
                _table.PutNotAllowKey(authKey ?? "");
                // context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await KeyNotAllow();
                return;
            }

            if (openKey.Available == false)
            {
                _table.PutNotAllowKey(authKey ?? "");
                // context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await KeyNotAllow("当前密钥被禁用");
                return;
            }


            context.Items["RequestKey"] = openKey;

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
        }

        await _next(context);
    }
}