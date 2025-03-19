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
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService teamService;

        public TeamsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        /// <summary>
        /// 添加组
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<BaseReponse> AddNew([FromBody] Team team) => teamService.AddNewAsync(team);


        /// <summary>
        /// 获取组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetSigle(long id) => teamService.GetSingleAsync(id);

        /// <summary>
        /// 获取组列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetAll([FromQuery] QueryParametersRequest request) => teamService.GetAllAsync(request);


        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public Task<BaseReponse> Delete(long id) => teamService.DeleteAsync(id);

        /// <summary>
        /// 更新组
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        [HttpPut]
        public Task<BaseReponse> Update([FromBody] Team team) => teamService.UpdateAsync(team);
    }
}
