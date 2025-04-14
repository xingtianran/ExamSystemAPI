using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Helper.Filter;
using ExamSystemAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public Task<BaseReponse> Register([FromQuery] RegisterRequest request) => userService.RegisterAsync(request);

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
        public Task<BaseReponse> GetAllAsync([FromQuery] QueryUsersParametersRequest request) => userService.GetAllAsync(request);

        /// <summary>
        /// 加入组
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> JoinTeam([FromQuery] JoinTeamRequest request) => userService.JoinTeamAsync(request);

        /// <summary>
        /// 检查登录状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> CheckStatus() => userService.CheckStatusAsync();

        /// <summary>
        /// 获取角色内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetRoles() => userService.GetRolesAsync();

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public Task<BaseReponse> ResetPwd(long userId, string newPwd) => userService.ResetPwdAsync(userId, newPwd);

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        [Authorize(Roles = "admin")]
        public Task<BaseReponse> LockUser(long userId) => userService.LockUserAsync(userId);

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        [Authorize(Roles = "admin")]
        public Task<BaseReponse> UnLockUser(long userId) => userService.UnLockUser(userId);

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetCount() => userService.GetCountAsync();

        /// <summary>
        /// 获取我的考试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetMyExam() => userService.GetMyExam();

        /// <summary>
        /// 获取考试详情信息
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet("{paperId}/{teamId}")]
        public Task<BaseReponse> GetExamDetail(long paperId, long teamId) => userService.GetExamDetail(paperId, teamId);


        /// <summary>
        /// 批改试卷并记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<BaseReponse> MarkPaper([FromBody]MarkPaperRequest request) => userService.MarkPaper(request);
    }
}
