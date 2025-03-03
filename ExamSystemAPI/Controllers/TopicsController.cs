using ExamSystemAPI.Extensions;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystemAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "teacher,admin")]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService topicService;

        public TopicsController(ITopicService topicService)
        {
            this.topicService = topicService;
        }

        /// <summary>
        /// 添加题目
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ApiResponse> AddNew([FromBody] Topic topic) => topicService.AddNew(topic);
    }
}
