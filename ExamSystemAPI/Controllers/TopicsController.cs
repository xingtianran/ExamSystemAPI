using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystemAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "admin,teacher")]
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
        public Task<BaseReponse> AddNew([FromBody] Topic topic) => topicService.AddNewAsync(topic);


        /// <summary>
        /// 获取题目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetSigle(long id) => topicService.GetSingleAsync(id);

        /// <summary>
        /// 获取题目列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetAll([FromQuery]QueryParametersRequest request) => topicService.GetAllAsync(request);


        /// <summary>
        /// 删除题目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public Task<BaseReponse> Delete(long id) => topicService.DeleteAsync(id);

        /// <summary>
        /// 更新题目
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPut]
        public Task<BaseReponse> Update([FromBody]Topic topic) => topicService.UpdateAsync(topic);

    }
}
