﻿using Dispatcher.FakeGpt;
using Dispatcher.Models;
using Dispatcher.Models.Openai;
using Dispatcher.Models.Requests;
using Microsoft.Extensions.Options;

namespace Dispatcher.Middlewares.api;

// 根据 一定的策略从动态密钥池中选取一个密钥和请求地址
public class RequestChooserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly KeyPoolRepository _repository;
    private readonly ServerBaseLimit _limit;
    public RequestChooserMiddleware(RequestDelegate next,KeyPoolRepository repository,IOptions<ServerBaseLimit> limit)
    {
        _next = next;
        _repository = repository;
        _limit = limit.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var random = new Random();
        if ((bool)(context.Items["Free"] ?? false))
        {
            var transferKey = new PoolKey()
            {
                PoolKeyId = long.MaxValue,
                Cipher = (string?)context.Items["RequestKey"],
                HandHosts = (string?)context.Items["ApiUrl"],
                Available = true,

            };
            context.Items["PickedKey"] = transferKey;
        }
        else
        {
            if (_repository.Count < 1)
            {
                var delayParts = _limit.WaitSeconds * 1000 / 10;
                var part = 0;
                while (part <= 10 && _repository.Count < 1)
                {
                    part++;
                    await Task.Delay(delayParts);
                }

                if (_repository.Count < 1)
                {
                    var completion = Completion.GetDefaultOrExample($"已经等待{_limit.WaitSeconds}秒\n" +
                                                                    $"当前动态池中没有实际请求密钥，请重试。（程序错误）"+
                                                                    $"\n这往往是因为IIS的应用程序池正在重新启动导致的，IIS会在长时间没有请求时，回收应用程序池。" +
                                                                    $"\n 不过，随着当前请求，应用程序池将会重新启动，请耐心等待或者重新请求。" +
                                                                    $"\n 本网站配置了Hangfire定时任务来进行密钥加载，所以可能会加载若干秒。" +
                                                                    $"\n 请坐和放宽。");
                    await GptReply.Reply(context,completion);
                    return;
                }
            }
            var index = random.Next(_repository.Count);
            var pickedKey = _repository.OpenPoolKeys[index];
            var copyKey = pickedKey.Clone();
            if (copyKey?.Available == false)
            {
                var completion = Completion.GetDefaultOrExample("数据异常，请联系管理员！");
                await GptReply.Reply(context,completion);
                return;
            }
            context.Items["PickedKey"] = copyKey;
        }
        await _next(context);
    }
}