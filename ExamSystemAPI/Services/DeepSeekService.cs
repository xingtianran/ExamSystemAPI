using Newtonsoft.Json;
using RestSharp;

namespace ExamSystemAPI.Services
{
    public class DeepSeekService
    {

        private readonly string URL = "https://api.deepseek.com/chat/completions";

        private readonly string TOKEN = "sk-2257e296dc95446e82c6a4ac7a0891d1";

        public async Task<string> PostInfoAsync(string userSolution, string solution) {
            var client = new RestClient();
            var request = new RestRequest(URL, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {TOKEN}");
            var body = $@"
                        {{
                          ""messages"": [
                            {{
                              ""content"": ""You are a helpful assistant"",
                              ""role"": ""system""
                            }},
                            {{
                              ""content"": ""请对比答案是否相似，差不多就行，大致有这个意思就行，正确答案：{solution}，用户提交的答案：{userSolution}，返回的content中是一个json格式数据，，有两个key，一个是detail分析详情，一个是solution得出答案，solution只有正确或者错误。"",
                              ""role"": ""user""
                            }}
                          ],
                          ""model"": ""deepseek-chat"",
                          ""frequency_penalty"": 0,
                          ""max_tokens"": 2048,
                          ""presence_penalty"": 0,
                          ""response_format"": {{
                            ""type"": ""text""
                          }},
                          ""stop"": null,
                          ""stream"": false,
                          ""stream_options"": null,
                          ""temperature"": 1,
                          ""top_p"": 1,
                          ""tools"": null,
                          ""tool_choice"": ""none"",
                          ""logprobs"": false,
                          ""top_logprobs"": null
                        }}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Content!)!;
            string jsonStr = data.choices[0].message.content;
            jsonStr = CleanJsonString(jsonStr);
            // 获得solution
            dynamic result = JsonConvert.DeserializeObject<dynamic>(jsonStr)!;
            return result.solution;
        }

        private static string CleanJsonString(string text)
        {
            // 移除```json和```标记
            string cleaned = text
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();
            return cleaned;
        }
    }
}
