using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Helper;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using ExamSystemAPI.Model.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExamSystemAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly ClaimHelper claimHelper;
        private readonly JWTHelper jwtHelper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly MyDbContext ctx;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, ClaimHelper claimHelper, JWTHelper jwtHelper, IHttpContextAccessor httpContextAccessor, MyDbContext ctx)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.claimHelper = claimHelper;
            this.jwtHelper = jwtHelper;
            this.httpContextAccessor = httpContextAccessor;
            this.ctx = ctx;
        }

        /// <summary>
        /// 初始化系统
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> InitAsync()
        {
            try
            {
                // 初始化角色
                Role? adminRole = await roleManager.FindByNameAsync("admin");
                if (adminRole == null)
                {
                    adminRole = new Role { Name = "admin" };
                    await roleManager.CreateAsync(adminRole);
                }
                Role? studentRole = await roleManager.FindByNameAsync("student");
                if (studentRole == null)
                {
                    studentRole = new Role { Name = "student" };
                    await roleManager.CreateAsync(studentRole);
                }
                Role? teacherRole = await roleManager.FindByNameAsync("teacher");
                if (teacherRole == null)
                {
                    teacherRole = new Role { Name = "teacher" };
                    await roleManager.CreateAsync(teacherRole);
                }
                // 初始化管理员账号
                User? admin = await userManager.FindByNameAsync("admin");
                if (admin == null)
                {
                    admin = new User { UserName = "admin"};
                    await userManager.CreateAsync(admin);
                }
                // 设置默认密码
                await userManager.AddPasswordAsync(admin, "123456").CheckAsync();
                // 关联管理员和管理员权限
                if (!await userManager.IsInRoleAsync(admin, "admin")) {
                    await userManager.AddToRoleAsync(admin, "admin").CheckAsync();
                }
                return new ApiResponse(200, "初始化成功");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }


        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseReponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                string username = request.UserName;
                string password = request.Password;
                string role = request.Role;
                if (string.IsNullOrEmpty(username)) 
                    return new ApiResponse(400, "用户名不能为空");
                if (string.IsNullOrEmpty(password)) 
                    return new ApiResponse(400, "密码不能为空");
                if (string.IsNullOrEmpty(role)) 
                    return new ApiResponse(400, "角色不能为空");
                User? user = await userManager.FindByNameAsync(username);
                if (user != null)
                    return new ApiResponse(400, "用户名已注册");
                // 添加用户
                user = new User { UserName = username };
                await userManager.CreateAsync(user).CheckAsync();
                await userManager.AddPasswordAsync(user, password).CheckAsync();
                // 2 teacher 1 student
                if (role == "2") role = "teacher";
                else role = "student";
                await userManager.AddToRoleAsync(user, role).CheckAsync();

                return new ApiResponse(200, "注册成功");
            }
            catch (Exception ex) { 
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<BaseReponse> LoginAsync(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username)) return new ApiResponse(400, "用户名不能为空");
                if (string.IsNullOrEmpty(password)) return new ApiResponse(400, "密码不能为空");
                User? user = await userManager.FindByNameAsync(username);
                if (user == null)
                    return new ApiResponse(400, "用户名或密码错误");
                if (await userManager.IsLockedOutAsync(user))
                {
                    return new ApiResponse(400, "用户已锁定,解锁日期:" + await userManager.GetLockoutEnabledAsync(user));
                }
                if (!await userManager.CheckPasswordAsync(user, password))
                {
                    await userManager.AccessFailedAsync(user);
                    return new ApiResponse(400, "用户名或密码错误");
                }
                // 重置登录失败次数
                await userManager.ResetAccessFailedCountAsync(user);
                // JWT版本加一
                user.JWTVersion++;
                await userManager.UpdateAsync(user);
                // 生成claims
                List<Claim> claims = await claimHelper.Model2ClaimsAsync(user);
                // 生成jwt
                string jwt = jwtHelper.GenerateJWT(claims);
                return new ApiResponse(200, jwt);
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> LogoutAsync()
        {
            try
            {
                string id = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                User? user = await userManager.FindByIdAsync(id);
                // JWT版本加一，之前版本失效
                user!.JWTVersion++;
                await userManager.UpdateAsync(user);
                return new ApiResponse(200, "退出成功");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 查询全部用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetAllAsync(QueryParametersRequest request) {
            try
            {
                int page = request.Page;
                int size = request.Size;
                int startIndex = (page - 1) * size;
                var baseSet = userManager.Users;
                var data = await baseSet.Skip(startIndex).Take(size).ToListAsync();
                var count = await baseSet.CountAsync();
                var totalPage = (int)Math.Ceiling(count * 1.0 / size);
                List<UserInfoResponse> response = new List<UserInfoResponse>();
                foreach (var user in data)
                {
                    UserInfoResponse userInfo = new UserInfoResponse();
                    userInfo.Id = user.Id;
                    userInfo.UserName = user.UserName!;
                    userInfo.Avatar = user.Avatar;
                    // 查询用户下的角色
                    var roles = await userManager.GetRolesAsync(user);
                    userInfo.Roles = roles;
                    response.Add(userInfo);
                }
                return new PageInfoResponse<UserInfoResponse>(200, "获取成功", page, size, totalPage, response);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 加入组
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseReponse> JoinTeamAsync(JoinTeamRequest request)
        {
            try
            {
                if (request.TeamId == 0) return new ApiResponse(400, "组编号不能为空");
                if (request.UserId == 0) return new ApiResponse(400, "用户编号不能为空");
                if (string.IsNullOrEmpty(request.Password)) return new ApiResponse(400, "密码不能为空");
                long teamId = request.TeamId;
                long userId = request.UserId;
                string password = request.Password;
                Team team = await ctx.Teams.SingleAsync(t => t.Id == teamId);
                if (team.Password != password)
                    return new ApiResponse(400, "密码不正确");
                User user = await ctx.Users.SingleAsync(u => u.Id == userId);
                team.Users.Add(user);
                user.Teams.Add(team);
                await ctx.Users.AddAsync(user);
                await ctx.Teams.AddAsync(team);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "加入成功") : new ApiResponse(500, "加入失败");
            }
            catch (Exception ex) { 
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 检查通过
        /// 返回当前用户
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> CheckStatus()
        {
            try
            {
                User user = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                UserInfoResponse userInfo = new UserInfoResponse();
                userInfo.Id = user.Id;
                userInfo.UserName = user.UserName!;
                userInfo.Avatar = user.Avatar;
                // 查询用户下的角色
                var roles = await userManager.GetRolesAsync(user);
                userInfo.Roles = roles;
                return new ApiResponse(200, "获取当前用户成功", userInfo);
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
