using Dispatcher.Models.openai;
using Newtonsoft.Json;

namespace Dispatcher.Endpoints
{
    public class TestTransferEndpoint
    {
        public async Task Endpoint(HttpContext context)
        {
            var json = "{\"id\":\"chatcmpl-7qFvNMxRuZTgRPGs4rjk3fMebsFSg\",\"object\":\"chat.completion.chunk\",\"created\":1692688625,\"model\":\"gpt-3.5-turbo-0301\",\"choices\":[{\"index\":0,\"delta\":{\"role\":\"assistant\",\"content\":\"\"},\"finish_reason\":null}]}";
            var completion = JsonConvert.DeserializeObject<Completion>(json);
            for (int i = 0; i < 10; i++)
            {
                completion!.Choices![0].Delta!.Content = $"hello{i}";
                await context.Response.WriteAsync("data: "+JsonConvert.SerializeObject(completion)+"\n\n");
            }

            await context.Response.WriteAsync("data: [DONE]");
        }
    }
}
