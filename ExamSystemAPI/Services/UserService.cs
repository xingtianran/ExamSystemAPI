using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Helper;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using ExamSystemAPI.Model.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
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
        public async Task<BaseReponse> GetAllAsync(QueryUsersParametersRequest request) {
            try
            {
                int page = request.Page;
                int size = request.Size;
                string? role = request.Role;
                string? userName = request.UserName;
                int startIndex = (page - 1) * size;
                IEnumerable<User> baseSet = await userManager.Users.ToListAsync();
                // 搜索条件
                if (!string.IsNullOrEmpty(role)) {
                    baseSet = await userManager.GetUsersInRoleAsync(role);
                }
                if (!string.IsNullOrEmpty(userName)) {
                    baseSet = baseSet.Where(b => b.UserName!.Contains(userName));
                }
                var data = baseSet.Skip(startIndex).Take(size);
                var count = baseSet.Count();
                var totalPage = (int)Math.Ceiling(count * 1.0 / size);
                List<UserInfoResponse> response = new List<UserInfoResponse>();
                foreach (var user in data)
                {
                    UserInfoResponse userInfo = new UserInfoResponse();
                    userInfo.Id = user.Id;
                    userInfo.UserName = user.UserName!;
                    userInfo.Avatar = user.Avatar;
                    // 是否锁定
                    userInfo.IsLock = user.LockoutEnd == null ? "1" : "0";
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
                // if (request.UserId == 0) return new ApiResponse(400, "用户编号不能为空");
                User user = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                //if (string.IsNullOrEmpty(request.Password)) return new ApiResponse(400, "密码不能为空");
                long teamId = request.TeamId;
                string password = request.Password;
                Team team = await ctx.Teams.SingleAsync(t => t.Id == teamId);
                //if (team.Password != password)
                //    return new ApiResponse(400, "密码不正确");
                //team.Users.Add(user);
                //user.Teams.Add(team);
                //await ctx.Users.AddAsync(user);
                //await ctx.Teams.AddAsync(team);

                // 更新组人数
                team.Count++;
                ctx.Teams.Update(team);

                int count = await ctx.Database.ExecuteSqlInterpolatedAsync($@"insert into T_Teams_Users(TeamsId, UsersId) values({teamId}, {user.Id})");
                return count > 0 ? new ApiResponse(200, "加入成功") : new ApiResponse(500, "加入失败");
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
        public async Task<BaseReponse> CheckStatusAsync()
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

        /// <summary>
        /// 获取角色内容
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> GetRolesAsync()
        {
            try
            {
                IEnumerable<Role> roles = await roleManager.Roles.ToListAsync();
                return new ApiResponse(200, "获取全部角色成功", roles);
            }
            catch (Exception ex) {

                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public async Task<BaseReponse> ResetPwdAsync(long userId, string newPwd)
        {
            try
            {
                if (userId == 0) return new ApiResponse(400, "用户编号不能为空");
                if (string.IsNullOrEmpty(newPwd)) return new ApiResponse(400, "新密码不能为空");
                User? user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return new ApiResponse(400, "用户不存在");
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, newPwd);
                // 重置完成之后，让用户JWTVersion加一
                user.JWTVersion++;
                var updateResult = await userManager.UpdateAsync(user);
                return result.Succeeded && updateResult.Succeeded ? new ApiResponse(200, "密码重置成功") : new ApiResponse(500, "密码重置失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<BaseReponse> LockUserAsync(long userId)
        {
            try
            {
                if (userId == 0) return new ApiResponse(400, "用户编号不能为空");
                User? user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return new ApiResponse(400, "用户不存在");
                // JWTVesion加一，让用户持有的失效
                user.JWTVersion++;
                // 设置一个足够大的时间
                user.LockoutEnd = DateTimeOffset.MaxValue;
                var result = await userManager.UpdateAsync(user);
                return result.Succeeded ? new ApiResponse(200, "锁定成功") : new ApiResponse(500, "锁定失败");
            }
            catch (Exception ex) { 
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<BaseReponse> UnLockUser(long userId)
        {
            try
            {
                if (userId == 0) return new ApiResponse(400, "用户编号不能为空");
                User? user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return new ApiResponse(400, "用户不存在");
                // 锁定时间置null
                user.LockoutEnd = null;
                // 重置登录失败次数
                user.AccessFailedCount = 0;
                var result = await userManager.UpdateAsync(user);
                return result.Succeeded ? new ApiResponse(200, "解锁成功") : new ApiResponse(500, "解锁失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取用户总数
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> GetCountAsync()
        {
            try
            {
                long count = await ctx.Users.LongCountAsync();
                return new ApiResponse(200, "获取总数成功", count);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取我的考试
        /// 根据用户获取群组，再根据群组获取发布的试卷
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> GetMyExam()
        {
            try
            {
                // 获取用户
                User user = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                // 获取组
                List<Team> teams = await ctx.Teams.Where(t => t.Users.Contains(user)).ToListAsync();
                // 根据组获取试卷
                List<Paper> papers = new List<Paper>();

                DbConnection conn = ctx.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }
                foreach (var team in teams)
                {
                    List<Paper> list = await ctx.Papers.Include(p => p.User).Include(p => p.Category).Where(p => p.Teams.Contains(team)).ToListAsync();
                    // 通过paperId和TeamId获取dealLine结束时间
                    foreach (var paper in list) {
                        PaperTeam paperTeam = new PaperTeam();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = $"select State, Deadline from T_Papers_Teams where PaperId = {paper.Id} and TeamId = {team.Id}";
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    paperTeam.State = reader.GetString(0);
                                    paperTeam.Deadline = reader.GetDateTime(1);
                                }
                            }
                        }
                        // 临时群组ID
                        paper.TempId = team.Id;
                        paper.State = paperTeam.State;
                        paper.TempTime = paperTeam.Deadline;
                    }
                    papers.AddRange(list);
                }
                // 将state为0的全部剔除
                papers = papers.Where(p => p.State != "0").ToList();
                return new ApiResponse(200, "获取成功", papers);
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取单个考试信息详情
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetExamDetail(long paperId, long teamId) {
            try
            {
                DbConnection conn = ctx.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }
                PaperTeam paperTeam = new PaperTeam();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"select Deadline from T_Papers_Teams where PaperId = {paperId} and TeamId = {teamId}";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            paperTeam.Deadline = reader.GetDateTime(0);
                        }
                    }
                }
                Paper paper = await ctx.Papers.Include(p => p.Topics).SingleAsync(p => p.Id == paperId);
                foreach (var topic in paper.Topics) {
                    topic.Papers = new List<Paper>();
                }
                paper.TempTime = paperTeam.Deadline;
                return paper != null ? new ApiResponse(200, "获取成功", paper) : new ApiResponse(500, "获取失败");
            }
            catch (Exception ex) { 
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 批改试卷并记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseReponse> MarkPaper(MarkPaperRequest request)
        {
            try
            {
                var comparator = new SemanticComparator();
                if (request.Id == 0) return new ApiResponse(400, "试卷编号不能为空");
                User currentUser = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!; 
                // 计算分数和答案
                List<MarkTopic> topics = request.Topics;
                // 错题记录
                List<ErrorRecord> errorList = new List<ErrorRecord>();
                double score = 0;
                foreach (var topic in topics)
                {
                    bool sign = false;
                    Topic topicFromDb = await ctx.Topics.FirstAsync(t => t.Id == topic.Id);
                    // 如果题目是简答题的话
                    if (topicFromDb.Type == "4")
                    {
                        float value = comparator.Compare(topicFromDb.Answer, topic.Answer);
                        // 如果这个值大于0.06的话就是正确的
                        if (value > 0.06)
                            score += topicFromDb.Score;
                        else
                            sign = true;
                    }
                    // 填空题
                    else if (topicFromDb.Type == "3") {
                        // 答案中放置答案的个数
                        double singleScore = topicFromDb.Score / Convert.ToInt32(topicFromDb.Answer);
                        if (topic.Answer.Contains('#'))
                        {
                            string[] answers = topic.Answer.Split('#');
                            for (int i = 0; i < answers.Length; i++) { 
                                float value = comparator.Compare(answers[i], topic.Answer);
                                if (value > 0.06)
                                    score += singleScore;
                                else
                                    sign = true;
                            }
                        }
                        else { 
                            float value = comparator.Compare(topic.Answer, topic.Answer);
                            if (value > 0.06)
                                score += singleScore;
                            else 
                                sign = true;
                        }
                    }
                    else
                    {
                        // 其他题型的话，直接对比答案
                        // 数据库中的答案与上传的答案相同
                        if (topicFromDb.Answer == topic.Answer)
                            score += topicFromDb.Score;
                        else
                            sign = true;
                    }

                    // 查看标记是true的话，就是错误的
                    if (sign) {
                        // 错误记录
                        ErrorRecord errorRecord = new ErrorRecord();
                        errorRecord.User = currentUser;
                        errorRecord.Topic = topicFromDb;
                        errorRecord.Answer = topic.Answer;
                        errorRecord.CreateTime = DateTime.Now;
                        errorRecord.UpdateTime = DateTime.Now;
                        errorList.Add(errorRecord);
                    }
                }
                Paper paper = await ctx.Papers.FirstAsync(p => p.Id == request.Id);
                // 插入考试记录表
                ExamRecord examRecord = new ExamRecord();
                examRecord.Name = paper.Title;
                examRecord.Score = score;
                examRecord.User = currentUser;
                examRecord.CreateTime = DateTime.Now;
                examRecord.UpdateTime = DateTime.Now;
                await ctx.ExamRecords.AddAsync(examRecord);
                // 插入错题记录表
                await ctx.ErrorRecords.AddRangeAsync(errorList);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "批改成功") : new ApiResponse(500, "批改失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }

        }

        /// <summary>
        /// 获取错题集信息
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> GetErrorSets()
        {
            User user = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
            var list = await ctx.ErrorRecords.Include(e => e.Topic).Where(e => e.User == user).ToListAsync();
            return new ApiResponse(200, "获取成功", list);
        }
    }
}
