using System.Net.Http.Headers;
using System.Text;
using Dispatcher.Models;

namespace Dispatcher.Endpoints;

public class TransferEndpoint
{
    public async Task Endpoint(HttpContext context)
    {
        // 对请求的验证可以放置在中间件。
        var path = context.Request.RouteValues["path"]?.ToString();//额外请求路径
        var poolKey = (PoolKey)context.Items["PickedKey"];
        if (poolKey == null)
        {
            await Error();
            return;
        }

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
        var requestBody = "";// 读取请求体
        using (var reader = new StreamReader(context.Request.Body))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, url);// 构建请求
        client.DefaultRequestHeaders.Add("Authorization",$"Bearer {poolKey.Cipher}");
        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        // 添加请求体和请求头
        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        var contentStream = await response.Content.ReadAsStreamAsync();
        var streamReader = new StreamReader(contentStream, Encoding.UTF8);
        var stringBuilder = new StringBuilder();
        while (!streamReader.EndOfStream)// 读取数据并返回
        {
            var line = await streamReader.ReadLineAsync();
            stringBuilder.AppendLine(line);
            await context.Response.WriteAsync(stringBuilder.ToString());
            stringBuilder.Clear();
        }

    }
}