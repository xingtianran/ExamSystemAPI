using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Helper;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using Microsoft.AspNetCore.Identity;
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

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, ClaimHelper claimHelper, JWTHelper jwtHelper, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.claimHelper = claimHelper;
            this.jwtHelper = jwtHelper;
            this.httpContextAccessor = httpContextAccessor;
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
                user.JWTVersion++;
                await userManager.UpdateAsync(user);
                return new ApiResponse(200, "退出成功");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
