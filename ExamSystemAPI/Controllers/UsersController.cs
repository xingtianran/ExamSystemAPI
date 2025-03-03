using ExamSystemAPI.Extensions;
using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Helper;
using ExamSystemAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystemAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
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
        public Task<ApiResponse> Init() => userService.Init();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [NotCheckJWTValiadation]
        public Task<ApiResponse> Register([FromQuery]RegisterRequest request) => userService.Register(request);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [NotCheckJWTValiadation]
        public Task<ApiResponse> Login(string username, string password) => userService.Login(username, password);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public Task<ApiResponse> Logout() => userService.Logout();

    }
}
