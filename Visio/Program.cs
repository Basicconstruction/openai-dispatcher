using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp;
using HtmlAgilityPack;

var sf = "fsdf sfsdlf";
var f = sf.Split(" ");
foreach (var s in f)
{
    Console.WriteLine(s);
}
// string input = "https://api.sdfsf.comsk-sfhsdlffsdfej";
// string pattern = @"(https?://[^/]+)(.*)sk-(.*)";
//
// Match match = Regex.Match(input, pattern);
//
// if (match.Success)
// {
//     string link = match.Groups[1].Value;
//     string rest = match.Groups[3].Value;
//
//     Console.WriteLine("Link: " + link);
//     Console.WriteLine("Rest: " + rest);
// }
// else
// {
//     Console.WriteLine("Invalid input string.");
// }

// HttpClient client = new HttpClient();
//
// // Set the request URL
// string url = "https://localhost:44343/v1/chat/completions";
// // string url = "https://api.chatanywhere.com.cn/v1/chat/completions";
//
// // Create the request headers
// // client.DefaultRequestHeaders.Add("Authorization", "Bearer sk-vctHAX39zlaODSTW6FJenC09lPUdVoHvEA3WbJmfDWdqw47S");
// client.DefaultRequestHeaders.Add("Authorization", "Bearer sk-34");
//
// client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.32.3");
// client.DefaultRequestHeaders.Add("Accept", "*/*");
// client.DefaultRequestHeaders.Add("Postman-Token", "9d476fe3-3211-4b34-965a-068312d6611f");
// client.DefaultRequestHeaders.Add("Host", "localhost:44343");
// client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
// client.DefaultRequestHeaders.Add("Connection", "keep-alive");
//
// // Create the request body
// string requestBody = @"{
//             ""model"": ""gpt-3.5-turbo"",
//             ""stream"": true,
//             ""messages"": [
//                 {
//                     ""role"": ""user"",
//                     ""content"": ""日本?""
//                 }
//             ]
//         }";
//
// // Create the HTTP content
// HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
// // content.Headers.Add("Content-Type", "application/json"); // Set the Content-Type header here
//
// // Send the POST request
// HttpResponseMessage response = await client.PostAsync(url, content);
//
// // Read the response
// string responseContent = await response.Content.ReadAsStringAsync();
//
// // Display the response
// Console.WriteLine(responseContent);

// var searchQuery = "java之父"; // 替换为你想要搜索的内容
// var url = $"https://www.bing.com/search?q={Uri.EscapeDataString(searchQuery)}&PC=U316&FORM=CHROMN";
//
// var httpClientHandler = new HttpClientHandler
// {
//     AllowAutoRedirect = false // 禁止自动重定向
// };
//
// using (var client = new HttpClient(httpClientHandler))
// {
//     var response = await client.GetAsync(url);
//     if (response.StatusCode == HttpStatusCode.Redirect)
//     {
//         var redirectUrl = response.Headers.Location.ToString();
//         response = await client.GetAsync(redirectUrl);
//     }
//
//     var content = await response.Content.ReadAsStringAsync();
//     var doc = new HtmlDocument();
//     doc.LoadHtml(content);
//
//
//     // 获取id为b_results的ol元素
//     var olElement = doc.GetElementbyId("b_results");
//
//     if (olElement != null)
//     {
//         // 获取ol元素下class包含b_algo的所有li元素
//         var liElements = olElement.SelectNodes(".//li[contains(@class, 'b_algo') or contains(@class, 'bg_ans')]");
//
//         if (liElements != null)
//         {
//             foreach (var liElement in liElements)
//             {
//                 // 获取li元素内部的文本内容
//                 var textContent = liElement.InnerText.Trim();
//
//                 // 如果文本内容不为空，则进行处理
//                 if (!string.IsNullOrEmpty(textContent))
//                 {
//                     // 处理文本内容
//                     Console.WriteLine(textContent);
//                 }
//             }
//         }
//     }
// }
