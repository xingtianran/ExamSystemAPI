using ExamSystemAPI.Extensions.Request;
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
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService teamService;

        public TeamsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        /// <summary>
        /// 添加群组
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<BaseReponse> AddNew([FromBody] Team team) => teamService.AddNewAsync(team);


        /// <summary>
        /// 获取群组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetSigle(long id) => teamService.GetSingleAsync(id);

        /// <summary>
        /// 获取群组列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetAll([FromQuery] QueryTeamsParametersRequest request) => teamService.GetAllAsync(request);


        /// <summary>
        /// 删除群组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<BaseReponse> Delete(long id) => teamService.DeleteAsync(id);

        /// <summary>
        /// 更新群组
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        [HttpPut]
        public Task<BaseReponse> Update([FromBody] Team team) => teamService.UpdateAsync(team);

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Task<BaseReponse> UpdateState(long id) => teamService.UpdateStateAsync(id);

        /// <summary>
        /// 获取全部群组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetCount() => teamService.GetCountAsync();


        /// <summary>
        /// 获取全部群组（不分页）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetList() => teamService.GetListAsync();

    }
}
