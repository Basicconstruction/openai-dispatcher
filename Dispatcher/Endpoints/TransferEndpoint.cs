using System.Net.Http.Headers;
using System.Text;
using Dispatcher.Models;
using System;
using Dispatcher.Models.Requests;

namespace Dispatcher.Endpoints;

public class TransferEndpoint
{
    public async Task Endpoint(HttpContext context,DataContext data,KeyPoolRepository repository,DynamicTable table)
    {
        // 对请求的验证可以放置在中间件。
        var path = context.Request.RouteValues["path"]?.ToString();//额外请求路径
        var poolKey = (PoolKey)context.Items["PickedKey"];
        if (poolKey == null)
        {
            await Error();
            return;
        }
        await context.Response.WriteAsync("Process "+data.GetHashCode());
        async Task Error()
        {
            await context.Response.WriteAsync("服务器内部错误，找不到可用的池主机或者池key");
        }
        string baseUrl = "";
        if (poolKey.HandHosts != null)
        {
            baseUrl = poolKey.HandHosts;
        }
        else if(poolKey.AvailableHosts!=null)
        {
            baseUrl = poolKey.AvailableHosts[new Random().Next(poolKey.AvailableHosts.Count)];
        }
        else
        {
            await Error();
            return;
        }

        var url = baseUrl+"/v1/"+path;//拼接代理地址
        var requestBody = (string)(context.Items["body"] ?? "");

        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, url);// 构建请求
        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {poolKey.Cipher}");
        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        // 添加请求体和请求头
        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        var contentStream = await response.Content.ReadAsStreamAsync();

        // 设置转发响应的Content-Type头
        try
        {
            context.Response.ContentType = response.Content.Headers.ContentType.ToString();
        }
        catch
        {//ignore

        }

        if ((int)response.StatusCode != StatusCodes.Status200OK)
        {
            var dk = data.PoolKeys.Where(p => p.PoolKeyId == poolKey.PoolKeyId).FirstOrDefault();
            if (dk != null)
            {
                repository.RemovePoolKey(dk);
                dk.Available = false;
            }

            data.SaveChanges();
        }
        table.Log($"使用的私有池密钥Id： {poolKey.PoolKeyId}\n" +
                  $"使用的主机是： {poolKey.HandHosts}" +
                  $"密钥是： {poolKey.Cipher?[..7]}");

        var buffer = new byte[50];
        int bytesRead;
        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await context.Response.Body.WriteAsync(buffer, 0, bytesRead);
            await context.Response.Body.FlushAsync();
        }
        // var originalContent = await response.Content.ReadAsStringAsync();
        // context.Response.ContentType = response!.Content!.Headers!.ContentType!.ToString();
        // await context.Response.WriteAsync(originalContent);

    }
}