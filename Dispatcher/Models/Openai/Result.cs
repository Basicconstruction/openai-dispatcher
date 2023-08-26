using Newtonsoft.Json;

namespace Dispatcher.Models.Openai;

public class Choice
{
    [JsonProperty("index")]
    public int Index { get; set; }

    [JsonProperty("delta")]
    public Delta? Delta { get; set; }

    [JsonProperty("finish_reason")]
    public object? FinishReason { get; set; }
}

public class Delta
{
    [JsonProperty("role")]
    public string? Role { get; set; }

    [JsonProperty("content")]
    public string? Content { get; set; }
}

public class Completion
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("object")]
    public string? Object { get; set; }

    [JsonProperty("created")]
    public int Created { get; set; }

    [JsonProperty("model")]
    public string? Model { get; set; }

    [JsonProperty("choices")]
    public List<Choice>? Choices { get; set; }

    public static Completion GetDefaultOrExample(string? content)
    {
        var json = "{\"id\":\"chatcmpl-7qFvNMxRuZTgRPGs4rjk3fMebsFSg\",\"object\":\"chat.completion.chunk\",\"created\":1692688625,\"model\":\"gpt-3.5-turbo-0301\",\"choices\":[{\"index\":0,\"delta\":{\"role\":\"assistant\",\"content\":\"\"},\"finish_reason\":null}]}";
        var completion = JsonConvert.DeserializeObject<Completion>(json);
        completion!.Choices![0].Delta!.Content = content;
        return completion!;
    }
}