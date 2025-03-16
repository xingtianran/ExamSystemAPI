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
    public class PaperService : IPaperService
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly MyDbContext ctx;

        public PaperService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, MyDbContext ctx)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.ctx = ctx;
        }

        /// <summary>
        /// 增加试卷
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        public async Task<BaseReponse> AddNewAsync(Paper paper)
        {
            try
            {
                if (string.IsNullOrEmpty(paper.Title)) return new ApiResponse(400, "试卷标题不能为空");
                if (paper.CategoryId == 0) return new ApiResponse(400, "类目编号不能为空");
                // 填充数据
                paper.User = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                // 取出完整数据放入paper中，插入数据库，关联起来
                List<Topic> topics = await ctx.Topics.Where(t => paper.TopicIds.Contains(t.Id)).ToListAsync();
                paper.Topics = topics;
                // 添加试卷，deadline不设置
                paper.CreateTime = DateTime.Now;
                paper.UpdateTime = DateTime.Now;
                await ctx.Papers.AddAsync(paper);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "增加成功") : new ApiResponse(500, "增加失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 删除试卷（禁用试卷）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> DeleteAsync(long id)
        {
            try
            {
                if (id == 0) return new ApiResponse(400, "试卷编号不能为空");
                Paper paper = await ctx.Papers.SingleAsync(p => p.Id == id);
                paper.State = "0";
                ctx.Papers.Update(paper);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "试卷禁用成功") : new ApiResponse(500, "试卷禁用失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取全部试卷
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
                var baseSet = ctx.Papers;
                var data = await baseSet.Skip(startIndex).Take(size).ToListAsync();
                var count = await baseSet.CountAsync();
                var totalPage = (int)Math.Ceiling(count * 1.0 / size);
                return new PageInfoResponse<Paper>(200, "获取成功", page, size, totalPage, data);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取单个试卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetSingleAsync(long id)
        {
            if (id == 0) return new ApiResponse(400, "试卷编号不能为空");
            Paper paper = await ctx.Papers.SingleAsync(p => p.Id == id);
            return paper != null ? new ApiResponse(200, "获取成功", paper) : new ApiResponse(500, "获取失败");
        }

        public Task<BaseReponse> UpdateAsync(Paper paper)
        {
            throw new NotImplementedException();
        }
    }
}
