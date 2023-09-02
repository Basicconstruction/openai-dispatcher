using System.Text;
using Dispatcher.FakeGpt;
using HtmlAgilityPack;

namespace Dispatcher.Endpoints.Worker;

public abstract class BingSearch
{
    public static async Task<string> GetContentAsync(HttpContext context,string last)
    {
        var searchQuery = last[..^1];
            await GptReply.ReplyStart(context,"正在使用搜索模式\n\n");
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 " +
                                                            "(Windows NT 10.0; Win64; x64) " +
                                                            "AppleWebKit/537.36 (KHTML, like Gecko) " +
                                                            "Chrome/116.0.0.0 Safari/537.36");
            string encodedQuery = Uri.EscapeDataString(searchQuery);
            // string urlToSearch = $"https://www.bing.com/search?q={encodedQuery}&PC=U316&FORM=CHROMN";
            string urlToSearch = $"http://www.bing.com/search?q=" +
                                 $"{encodedQuery}&scope=web&mkt=en-US&ui=zh-CN&ADLT=OFF";
            var response = await client.GetAsync(urlToSearch);
            var content = await response.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            // 获取id为b_results的ol元素
            var olElement = doc.GetElementbyId("b_results");
            var stringBuilder = new StringBuilder();
            if (olElement != null)
            {
                // 获取ol元素下class包含b_algo的所有li元素
                var liElements = olElement.
                    SelectNodes(".//li[contains(@class, 'b_algo') or contains(@class, 'bg_ans')]");

                if (liElements != null)
                {
                    foreach (var liElement in liElements)
                    {
                        // 获取li元素内部的文本内容
                        var textContent = liElement.InnerText.Trim();

                        // 如果文本内容不为空，则进行处理
                        if (!string.IsNullOrEmpty(textContent))
                        {
                            stringBuilder.Append(textContent);
                        }
                    }
                }
            }
            await GptReply.ReplyJust(context, "搜索完毕，正在请求api处理\n\n");
            // var newTemplate = $"重新组织下面的搜索结果，给出关于\"{last}\"的搜索的主要内容，并标注引用链接。\n文本中链接位于一段内容的开头,当然如果有具体的对应于\"{last}\"结果，请把它放在首位。" +
            //                   $"比如查询现在的时间，如果下面的内容中包含时间就需要返回时间。\n" +
            //                   $"{stringBuilder}";
            var newTemplate = $"处理data后面的数据，并处理使得符合关于问题\"{last}\"的答案." +
                              $"当然如果我没有提供数据，请在回答中说明，并且给我提供你的解答。\n data: " +
                              $"{stringBuilder}";
            return newTemplate;
    }
}