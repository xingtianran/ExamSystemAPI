using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystemAPI.Controllers.Portal
{
    [Route("api/portal/[controller]/[action]")]
    [ApiController]
    public class TeamsPortalController : ControllerBase
    {

        private readonly ITeamService teamService;

        public TeamsPortalController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        /// <summary>
        /// 获取群组列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetAll([FromQuery] QueryParametersRequest request) => teamService.GetAllAsync(request);

        /// <summary>
        /// 获取我的群组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetMy() => teamService.GetMyAsync(); 
    }
}
