using Dispatcher.Models.Openai;

namespace Dispatcher.FakeGpt;

public class FakeReply
{
    public static string Reply(string request)
    {
        string pre = $"今天是{DateTime.Now.ToString()}\n很高兴你使用我们的网站的服务\n";
        string reply = $"非常感谢您提交的 \"{request}\" 请求。" +
                       $"我们已经收到了您的反馈，并会尽快处理。" +
                       $"在此期间，我们将认真分析您的需求，并尽最大努力为您提供最优质的服务。" +
                       $"同时，如果您有任何其他问题或建议，也欢迎随时联系我们。再次感谢您的支持和信任！\n\n" +
                       $"你的智障助手,Fake gpt";
        return pre+reply;
    }
    public static string Reply(EasyModel? model)
    {
        return Reply(model?.Messages[^1].Content ?? "hello world");
    }
    public static async Task<string> ReplyAsync(string request)
    {
        return null;
    }

    public static async Task<string> ReplyAsync(EasyModel model)
    {
        return await ReplyAsync(model.Messages[^1].Content);
    }
}