using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using ExamSystemAPI.Model.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExamSystemAPI.Services
{
    public class TeamService : ITeamService
    {

        private readonly MyDbContext ctx;
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TeamService(MyDbContext ctx, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.ctx = ctx;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 创建组
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public async Task<BaseReponse> AddNewAsync(Team team)
        {
            try
            {
                if (string.IsNullOrEmpty(team.Name)) return new ApiResponse(400, "组名称不能为空");
                if (string.IsNullOrEmpty(team.Password)) return new ApiResponse(400, "组密码不能为空");
                team.CreateUser = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                team.CreateTime = DateTime.Now;
                team.UpdateTime = DateTime.Now;
                await ctx.Teams.AddAsync(team);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "创建成功") : new ApiResponse(500, "创建失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }

        }

        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> DeleteAsync(long id)
        {
            try
            {
                if (id == 0) return new ApiResponse(400, "组编号不能为空");
                Team team = await ctx.Teams.SingleAsync(t => t.Id == id);
                ctx.Teams.Remove(team);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "删除成功") : new ApiResponse(500, "删除失败");
            }
            catch (Exception ex) { 
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取全部组
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetAllAsync(QueryTeamsParametersRequest request)
        {
            try
            {
                int page = request.Page;
                int size = request.Size;
                string? name = request.Name;
                int startIndex = (page - 1) * size;
                IQueryable<Team> baseSet = ctx.Teams.Include(b => b.CreateUser).Include(b => b.Users);
                if (!string.IsNullOrEmpty(name))
                    baseSet = baseSet.Where(b => b.Name.Contains(name));
                var data = await baseSet.Skip(startIndex).Take(size).ToListAsync();
                var count = await baseSet.CountAsync();
                var totalPage = (int)Math.Ceiling(count * 1.0 / size);
                return new PageInfoResponse<Team>(200, "获取成功", page, size, totalPage, data);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetSingleAsync(long id)
        {
            try
            {
                if (id == 0) return new ApiResponse(400, "组编号不能为空");
                Team team = await ctx.Teams.SingleAsync(t => t.Id == id);
                return team != null ? new ApiResponse(200, "获取成功", team) : new ApiResponse(500, "获取失败");
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 更新组
        /// 组名称、组密码、组状态
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public async Task<BaseReponse> UpdateAsync(Team team)
        {
            try
            {
                if (string.IsNullOrEmpty(team.Name)) return new ApiResponse(400, "组名称不能为空");
                if (string.IsNullOrEmpty(team.Password)) return new ApiResponse(400, "组密码不能为空");
                Team teamFromDB = await ctx.Teams.SingleAsync(t => t.Id == team.Id);
                teamFromDB.Name = team.Name;
                teamFromDB.Password = team.Password;
                teamFromDB.UpdateTime = DateTime.Now;
                ctx.Teams.Update(teamFromDB);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "更新成功") : new ApiResponse(500, "更新失败");
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 更改群组状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> UpdateStateAsync(long id)
        {
            try
            {
                if (id == 0) return new ApiResponse(400, "群组编号不能为空");
                Team team = await ctx.Teams.SingleAsync(t => t.Id == id);
                team.State = team.State == "1" ? "0" : "1";
                ctx.Teams.Update(team);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "更改成功") : new ApiResponse(500, "更改失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取群组总数
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> GetCountAsync()
        {
            try
            {
                long count = await ctx.Teams.LongCountAsync();
                return new ApiResponse(200, "获取总数成功", count);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取全部群组（不分页）
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> GetListAsync()
        {
            try
            {
                IEnumerable<Team> teams = await ctx.Teams.ToListAsync();
                return new ApiResponse(200, "获取成功", teams);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }


        /// <summary>
        /// 获取全部群组（门户）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetAllAsync(QueryParametersRequest request)
        {
            try
            {
                int page = request.Page;
                int size = request.Size;
                int startIndex = (page - 1) * size;
                IQueryable<Team> baseSet = ctx.Teams.Include(b => b.CreateUser);
                var data = await baseSet.Skip(startIndex).Take(size).ToListAsync();
                var count = await baseSet.CountAsync();
                var totalPage = (int)Math.Ceiling(count * 1.0 / size);
                return new PageInfoResponse<Team>(200, "获取成功", page, size, totalPage, data);
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取我的群组
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> GetMyAsync() {
            try
            {
                User user = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                IEnumerable<Team> teams = await ctx.Teams.Where(t => t.Users.Contains(user)).ToListAsync();
                foreach (var team in teams)
                {
                    team.Users = new List<User>();
                }
                return new ApiResponse(200, "获取成功", teams);
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }

        }
    }
}
