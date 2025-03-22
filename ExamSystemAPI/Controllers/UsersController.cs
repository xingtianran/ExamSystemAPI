using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Helper.Filter;
using ExamSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystemAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// 初始化系统
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NotCheckJWTValiadation]
        public Task<BaseReponse> Init() => userService.InitAsync();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [NotCheckJWTValiadation]
        public Task<BaseReponse> Register([FromQuery]RegisterRequest request) => userService.RegisterAsync(request);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [NotCheckJWTValiadation]
        public Task<BaseReponse> Login(string username, string password) => userService.LoginAsync(username, password);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public Task<BaseReponse> Logout() => userService.LogoutAsync();

        
        /// <summary>
        /// 查询全部用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetAllAsync([FromQuery]QueryParametersRequest request) => userService.GetAllAsync(request);

        /// <summary>
        /// 加入组
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> JoinTeam([FromQuery]JoinTeamRequest request) => userService.JoinTeamAsync(request);

        /// <summary>
        /// 检查登录状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> CheckStatus() => userService.CheckStatus();

    }
}
