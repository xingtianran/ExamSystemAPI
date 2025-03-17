﻿using ExamSystemAPI.Extensions.Request;
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
    public class PapersController : ControllerBase
    {
        private readonly IPaperService paperService;

        public PapersController(IPaperService paperService) { 
            this.paperService = paperService;
        }

        /// <summary>
        /// 添加试卷
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<BaseReponse> AddNew([FromBody] Paper paper) => paperService.AddNewAsync(paper);


        /// <summary>
        /// 获取试卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetSigle(long id) => paperService.GetSingleAsync(id);

        /// <summary>
        /// 获取试卷列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetAll([FromQuery] QueryParametersRequest request) => paperService.GetAllAsync(request);


        /// <summary>
        /// 删除试卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public Task<BaseReponse> Delete(long id) => paperService.DeleteAsync(id);

        /// <summary>
        /// 更新试卷
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        [HttpPut]
        public Task<BaseReponse> Update([FromBody]Paper paper) => paperService.UpdateAsync(paper);

        /// <summary>
        /// 发布试卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        public Task<BaseReponse> Publish([FromQuery]PublishPaperRequest request) => paperService.PublishAsync(request);
    }
}
