using ExamSystemAPI.Extensions;
using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using Microsoft.AspNetCore.Identity;

namespace ExamSystemAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// 初始化系统
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> Init()
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
                    admin = new User { UserName = "admin", PasswordHash = "admin" };
                    await userManager.CreateAsync(admin);
                }
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

        public async Task<ApiResponse> Register(RegisterRequest request)
        {
            string username = request.UserName;
            string password = request.Password;
            string role = request.Role;
            if (string.IsNullOrEmpty(username)) return new ApiResponse(400, "用户名不能为空");
            if (string.IsNullOrEmpty(password)) return new ApiResponse(400, "密码不能为空");
            if (string.IsNullOrEmpty(role)) return new ApiResponse(400, "角色不能为空");

            return null;
        }
    }
}
