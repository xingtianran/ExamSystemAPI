using ExamSystemAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExamSystemAPI.Test
{
    [TestClass]
    public class TestService
    {
        [TestMethod]
        public async Task Test01() { 
            DeepSeekService service = new DeepSeekService();
            string content = await service.PostInfoAsync("奥特曼不能打小怪兽", "奥特曼不能打小东西");
            // dynamic json = JsonConvert.DeserializeObject<dynamic>(content);
            // var obj = JsonConvert.DeserializeObject<dynamic>(content);
            Console.WriteLine(content.Trim());
            // dynamic json = JsonConvert.DeserializeObject<dynamic>(content);
            // Console.WriteLine(json.solution);
            // Console.WriteLine(json.choices[0].message.content);
            // Console.WriteLine(content);
        }


    }
}
