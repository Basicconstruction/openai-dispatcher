using Dispatcher.FakeGpt;
using Dispatcher.Models.Openai;
using Newtonsoft.Json;

namespace Dispatcher.Endpoints
{
    public class TestTransferEndpoint
    {
        public async Task Endpoint(HttpContext context)
        {

            var json = "{\"id\":\"chatcmpl-7qFvNMxRuZTgRPGs4rjk3fMebsFSg\",\"object\":\"chat.completion.chunk\",\"created\":1692688625,\"model\":\"gpt-3.5-turbo-0301\",\"choices\":[{\"index\":0,\"delta\":{\"role\":\"assistant\",\"content\":\"\"},\"finish_reason\":null}]}";
            var completion = JsonConvert.DeserializeObject<Completion>(json);

            var requestBody = "";// 读取请求体
            using (var reader = new StreamReader(context.Request.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }
            completion!.Choices![0].Delta!.Content = FakeReply.Reply(
                JsonConvert.DeserializeObject<EasyModel>(requestBody));

            await GptReply.Reply(context, completion);
        }
    }
}
