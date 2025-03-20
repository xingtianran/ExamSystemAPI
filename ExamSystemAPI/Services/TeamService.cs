using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using ExamSystemAPI.Model.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ExamSystemAPI.Services
{
    public class TeamService : ITeamService
    {

        private readonly MyDbContext ctx;
        public TeamService(MyDbContext ctx)
        {
            this.ctx = ctx;
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
        public async Task<BaseReponse> GetAllAsync(QueryParametersRequest request)
        {
            try
            {
                int page = request.Page;
                int size = request.Size;
                int startIndex = (page - 1) * size;
                var baseSet = ctx.Topics;
                var data = await baseSet.Skip(startIndex).Take(size).ToListAsync();
                var count = await baseSet.CountAsync();
                var totalPage = (int)Math.Ceiling(count * 1.0 / size);
                return new PageInfoResponse<Topic>(200, "获取成功", page, size, totalPage, data);
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
                teamFromDB.State = team.State;
                teamFromDB.UpdateTime = DateTime.Now;
                ctx.Teams.Update(teamFromDB);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "更新成功") : new ApiResponse(500, "更新失败");
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
