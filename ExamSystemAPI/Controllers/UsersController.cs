using ExamSystemAPI.Extensions;
using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using Microsoft.AspNetCore.Identity;
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
        public Task<ApiResponse> Init() => userService.Init();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ApiResponse> Register([FromQuery]RegisterRequest request) => userService.Register(request);
    }
}
