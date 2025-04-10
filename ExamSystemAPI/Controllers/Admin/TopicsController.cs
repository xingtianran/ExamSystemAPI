﻿using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystemAPI.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
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
        [HttpGet("{id}")]
        public Task<BaseReponse> GetSingle(long id) => topicService.GetSingleAsync(id);

        /// <summary>
        /// 获取题目列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetAll([FromQuery] QueryTopicsParametersRequest request) => topicService.GetAllAsync(request);


        /// <summary>
        /// 删除题目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<BaseReponse> Delete(long id) => topicService.DeleteAsync(id);

        /// <summary>
        /// 更新题目
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPut]
        public Task<BaseReponse> Update([FromBody] Topic topic) => topicService.UpdateAsync(topic);


        /// <summary>
        /// 添加题目到试卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<ApiResponse> AddTopic2Paper([FromQuery] AddTopic2PaperRequest request) => topicService.AddTopic2PaperAsync(request);

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Task<BaseReponse> UpdateState(long id) => topicService.UpdateStateAsync(id);

        /// <summary>
        /// 获取部分题目详情
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetPart([FromQuery] string ids) => topicService.GetPartAsync(ids);

        /// <summary>
        /// 获取全部题目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetCount() => topicService.GetCountAsync();

        /// <summary>
        /// 获取最新题目
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("{size}")]
        public Task<BaseReponse> GetNew(int size) => topicService.GetNewAsync(size);

    }
}
