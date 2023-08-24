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
}