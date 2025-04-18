﻿using ExamSystemAPI.Extensions.Request;
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
                // 查询出类目
                Category category = await ctx.Categories.SingleAsync(c => c.Id == topic.CategoryId);
                topic.Category = category;
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
        public async Task<BaseReponse> GetAllAsync(QueryTopicsParametersRequest request)
        {
            try
            {
                int page = request.Page;
                int size = request.Size;
                string? title = request.Title;
                string? type = request.Type;
                long categoryId = request.CategoryId;
                int startIndex = (page - 1) * size;
                IQueryable<Topic> baseSet = ctx.Topics.Include(t => t.Category).Include(t => t.User);
                if (!string.IsNullOrEmpty(title))
                    baseSet = baseSet.Where(t => t.Title.Contains(title));
                if (!string.IsNullOrEmpty(type))
                    baseSet = baseSet.Where(t => t.Type == type);
                if (categoryId != 0)
                    baseSet = baseSet.Where(t => t.Category.Id == categoryId);
                var data = await baseSet.Skip(startIndex).Take(size).ToListAsync();
                // 防止循环依赖
                for (int i = 0; i < data.Count; i++)
                {
                    data[i].Category.Parent = null;
                    data[i].Category.Children = new List<Category>();
                }
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
                topicFromDB.Type = topic.Type;
                topicFromDB.CategoryId = topic.CategoryId;
                topicFromDB.User = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                topicFromDB.Score = topic.Score;
                topicFromDB.Column1 = topic.Column1;
                topicFromDB.Column2 = topic.Column2;
                topicFromDB.Column3 = topic.Column3;
                topicFromDB.Column4 = topic.Column4;
                topicFromDB.Column5 = topic.Column5;
                topicFromDB.Column6 = topic.Column6;
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

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> UpdateStateAsync(long id) {
            try
            {
                if (id == 0) return new ApiResponse(400, "题目编号不能为空");
                Topic topic = await ctx.Topics.SingleAsync(t => t.Id == id);
                topic.State = topic.State == "1" ? "0" : "1";
                ctx.Topics.Update(topic);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "更改成功") : new ApiResponse(500, "更改失败");
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }


        /// <summary>
        /// 获取部分题目详情，题目编号用/号分隔
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetPartAsync(string ids)
        {
            try
            {
                if (string.IsNullOrEmpty(ids)) return new ApiResponse(400, "题目编号不能为空");
                long[] idList = ids.Split("/").Select(i => long.Parse(i)).ToArray();
                List<Topic> topics = await ctx.Topics.Where(t => idList.Contains(t.Id)).ToListAsync();
                return topics.Count > 0 ? new ApiResponse(200, "获取成功", topics) : new ApiResponse(500, "获取失败");
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取题目总数
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> GetCountAsync() {
            try
            {
                long count = await ctx.Topics.LongCountAsync();
                return new ApiResponse(200, "获取总数成功", count);
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取最新题目
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetNewAsync(int size)
        {
            try
            {
                var topics = await ctx.Topics.Include(t => t.Category).Include(c => c.User).Take(size).ToListAsync();
                // 防止循环依赖
                for (int i = 0; i < topics.Count; i++)
                {
                    topics[i].Category.Parent = null;
                    topics[i].Category.Children = new List<Category>();
                }
                return new ApiResponse(200, "获取成功", topics);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
