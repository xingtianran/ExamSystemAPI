using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using ExamSystemAPI.Model.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace ExamSystemAPI.Services
{
    public class TopicService : ITopicService
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly MyDbContext ctx;
        private readonly IMemoryCache cache;

        public TopicService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, MyDbContext ctx, IMemoryCache cache)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.ctx = ctx;
            this.cache = cache;
        }

        /// <summary>
        /// 新增题目
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task<BaseReponse> AddNewAsync(Topic topic)
        {
            try
            {
                if (string.IsNullOrEmpty(topic.Title)) return new ApiResponse(400, "题目标题不能为空");
                if (string.IsNullOrEmpty(topic.Content)) return new ApiResponse(400, "题目内容不能为空");
                if (string.IsNullOrEmpty(topic.Answer)) return new ApiResponse(400, "题目答案不能为空");
                if (string.IsNullOrEmpty(topic.Type)) return new ApiResponse(400, "题目类型不能为空");
                if (topic.CategoryId == 0) return new ApiResponse(400, "类目编号不能为空");
                // 填充数据
                topic.User = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                topic.CreateTime = DateTime.Now;
                topic.UpdateTime = DateTime.Now;
                await ctx.Topics.AddAsync(topic);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "添加成功") : new ApiResponse(500, "添加失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }


        /// <summary>
        /// 删除题目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> DeleteAsync(long id)
        {
            try
            {
                if (id == 0) return new ApiResponse(400, "删除题目");
                Topic topic = await ctx.Topics.FirstAsync(t => t.Id == id);
                ctx.Topics.Remove(topic);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "删除成功") : new ApiResponse(500, "删除失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取全部题目成功
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
        /// 获取成功
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetSingleAsync(long id)
        {
            try
            {
                if (id == 0) return new ApiResponse(400, "题目编号不能为空");
                Topic topic = await ctx.Topics.FirstAsync(t => t.Id == id);
                return topic != null ? new ApiResponse(200, "获取成功", topic) : new ApiResponse(400, "获取失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 修改题目
        /// 题目标题、题目内容、题目答案、类目编号
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task<BaseReponse> UpdateAsync(Topic topic)
        {
            try
            {
                if (topic.Id == 0) return new ApiResponse(400, "题目编号不能为空");
                if (string.IsNullOrEmpty(topic.Title)) return new ApiResponse(400, "题目标题不能为空");
                if (string.IsNullOrEmpty(topic.Content)) return new ApiResponse(400, "题目内容不能为空");
                if (string.IsNullOrEmpty(topic.Answer)) return new ApiResponse(400, "题目答案不能为空");
                if (topic.CategoryId == 0) return new ApiResponse(400, "类目编号不能为空");
                Topic topicFromDB = await ctx.Topics.FirstAsync(t => t.Id == topic.Id);
                topicFromDB.Title = topic.Title;
                topicFromDB.Content = topic.Content;
                topicFromDB.Answer = topic.Answer;
                topicFromDB.CategoryId = topic.CategoryId;
                topicFromDB.User = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                topicFromDB.Score = topic.Score;
                topicFromDB.UpdateTime = DateTime.Now;
                ctx.Topics.Update(topicFromDB);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "更新成功") : new ApiResponse(500, "更新失败");
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 添加题目到试卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResponse> AddTopic2PaperAsync(AddTopic2PaperRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.sign)) return Task.FromResult(new ApiResponse(400, "试卷唯一标识不能为空"));
                if (request.TopicId == 0) return Task.FromResult(new ApiResponse(400, "题目编号不能为空"));
                string sign = request.sign;
                long topicId = request.TopicId;
                // 将题目编号保存到内存，key为uuid值，保存时间为一个小时
                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                if (cache.TryGetValue(sign, out var value))
                    cache.Set(sign, value + "#" + topicId + "/" + DateTime.Now, cacheOptions);
                else
                    cache.Set(sign, topicId + "/" + DateTime.Now, cacheOptions);
                return Task.FromResult(new ApiResponse(200, "添加成功"));
            }
            catch (Exception ex) {
                return Task.FromResult(new ApiResponse(500, ex.Message));
            }
        }
    }
}
