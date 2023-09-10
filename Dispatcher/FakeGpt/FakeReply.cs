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
                       $"你的智障助手,Fake gpt\n" +
                       $"请联系站长获得密钥，或者可以使用自己的密钥使用方式\n" +
                       $"在密钥栏： 填写密钥时，如果时openai的密钥 在前面添加o,\n" +
                       $"比如osk-sdfsdferrejKFDdfsdl\n" +
                       $"如果是openai-sb的密钥，在前面添加s\n" +
                       $"如果是chatanywhere的密钥，在前面添加c\n" +
                       $"若果是https://api.example.com的密钥，在前面添加https://api.example.com，加个空格跟上密钥\n" +
                       $"比如https://api.example.com sk-kfcvme50.\n";
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