using System.Data.Common;
using System.Security.Claims;
using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using ExamSystemAPI.Model.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ExamSystemAPI.Services
{
    public class PaperService : IPaperService
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly MyDbContext ctx;
        private readonly IMemoryCache cache;

        public PaperService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, MyDbContext ctx, IMemoryCache cache)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.ctx = ctx;
            this.cache = cache;
        }

        /// <summary>
        /// 增加试卷
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        public async Task<BaseReponse> AddNewAsync(Paper paper, string sign)
        {
            try
            {
                if (string.IsNullOrEmpty(paper.Title)) return new ApiResponse(400, "试卷标题不能为空");
                if (paper.CategoryId == 0) return new ApiResponse(400, "类目编号不能为空");
                // 填充数据
                paper.User = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                // 取出完整数据放入paper中，插入数据库，关联起来
                // List<Topic> topics = await ctx.Topics.Where(t => paper.TopicIds.Contains(t.Id)).ToListAsync();
                // 在内存中取出该试卷下的题目编号
                // 题目编号/时间戳#题目编号/时间戳
                // 内存中的格式:1/1679212200#2/1679212210
                string? value = null;
                List<Topic> topics = new List<Topic>();
                if (cache.TryGetValue<string>(sign, out value))
                {
                    Topic? temp = null;
                    if (value != null)
                    {
                        string? id = null;
                        string? timestamp = null;
                        if (value.Contains('#'))
                        {
                            string[] idTimeArray = value.Split('#');
                            List<Topic> idTimeList = new List<Topic>();
                            foreach (string idTime in idTimeArray)
                            {
                                id = idTime.Split('/')[0];
                                timestamp = idTime.Split("/")[1];
                                // 时间戳转化为DateTime类型
                                idTimeList.Add(new Topic { Id = long.Parse(id), TempTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp)).DateTime });
                            }
                            foreach (Topic item in idTimeList)
                            {
                                temp = await ctx.Topics.SingleAsync(t => t.Id == item.Id);
                                temp.TempTime = item.TempTime;
                                topics.Add(temp);
                            }
                        }
                        else
                        {
                            id = value.Split('/')[0];
                            timestamp = value.Split('/')[1];
                            temp = await ctx.Topics.SingleAsync(t => t.Id == long.Parse(id));
                            temp.TempTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp)).DateTime;
                            topics.Add(temp);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                paper.Topics = topics!;
                paper.CreateTime = DateTime.Now;
                paper.UpdateTime = DateTime.Now;
                // 保存到关联表 PaperTopic
                List<PaperTopic> paperTopicList = new List<PaperTopic>();
                foreach (Topic topic in paper.Topics)
                {
                    PaperTopic paperTopic = new PaperTopic();
                    paperTopic.PaperId = paper.Id;
                    paperTopic.TopicId = topic.Id;
                    paperTopicList.Add(paperTopic);
                }
                // 保存到关联表
                await ctx.PaperTopics.AddRangeAsync(paperTopicList);
                // 保存到试卷表
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
                ctx.Papers.Remove(paper);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "删除成功") : new ApiResponse(500, "删除失败");
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
        public async Task<BaseReponse> GetAllAsync(QueryPapersParametersRequest request)
        {
            try
            {
                int page = request.Page;
                int size = request.Size;
                string? title = request.Title;
                long categoryId = request.CategoryId;
                int startIndex = (page - 1) * size;
                IQueryable<Paper> baseSet = ctx.Papers.Include(p => p.Category).Include(p => p.User);
                if (!string.IsNullOrEmpty(title))
                    baseSet = baseSet.Where(b => b.Title.Contains(title));
                if (categoryId != 0)
                    baseSet = baseSet.Where(b => b.CategoryId == categoryId);
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
            try
            {
                if (id == 0) return new ApiResponse(400, "试卷编号不能为空");
                Paper paper = await ctx.Papers.Include(p => p.User).Include(p => p.Category).SingleAsync(p => p.Id == id);
                return paper != null ? new ApiResponse(200, "获取成功", paper) : new ApiResponse(500, "获取失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 更新试卷
        /// 可以更新的内容：标题、分值、类目、题目
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        public async Task<BaseReponse> UpdateAsync(Paper paper)
        {
            try
            {
                if (paper.Id == 0) return new ApiResponse(400, "试卷编号不能为空");
                if (string.IsNullOrEmpty(paper.Title)) return new ApiResponse(400, "试卷标题不能为空");
                if (paper.CategoryId == 0) return new ApiResponse(400, "试卷类目不能为空");
                // if (paper.TopicIds.Count == 0) return new ApiResponse(400, "请添加题目");
                Paper paperFromDB = await ctx.Papers.SingleAsync(p => p.Id == paper.Id);
                paperFromDB.Title = paper.Title;
                paperFromDB.CategoryId = paper.CategoryId;
                // 填充题目数据
                // List<Topic> topics = await ctx.Topics.Where(t => paper.TopicIds.Contains(t.Id)).ToListAsync();
                // paperFromDB.Topics = topics;
                // 处理不合法数据
                paperFromDB.State = paper.State == "1" ? paper.State : "0";
                paperFromDB.UpdateTime = DateTime.Now;
                ctx.Papers.Update(paperFromDB);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "更新成功") : new ApiResponse(500, "更新失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 发布试卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseReponse> PublishAsync(PublishPaperRequest request)
        {
            try
            {
                if (request.PaperId == 0) return new ApiResponse(400, "试卷编号不能为空");
                if (request.TeamId == 0) return new ApiResponse(400, "组编号不能为空");
                // 将时间戳转化为本地时间
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(request.Deadline);
                // 将 UTC 时间转换为本地时间
                DateTimeOffset localDateTimeOffset = dateTimeOffset.ToLocalTime();
                DateTime examDeadline = localDateTimeOffset.DateTime;
                if (examDeadline < DateTime.Now) return new ApiResponse(400, "请设置正确时间");
                PaperTeam paperTeam = null;
                // 先查询，如果有该试卷编号和群组编号的记录的话，就更新记录值
                DbConnection conn = ctx.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"select PaperId, TeamId, State from T_Papers_Teams where PaperId = '{request.PaperId}' and TeamId = '{request.TeamId}'";
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            paperTeam = new PaperTeam();
                            paperTeam.PaperId = reader.GetInt64(0);
                            paperTeam.TeamId = reader.GetInt64(1);
                            paperTeam.State = reader.GetString(2);
                        }
                    }
                }
                int count = 0;
                if (paperTeam != null)
                {
                    count = await ctx.Database.ExecuteSqlInterpolatedAsync($@"update T_Papers_Teams set Deadline = {examDeadline}, State = '1' where PaperId = {paperTeam.PaperId} and TeamId = {paperTeam.TeamId}");
                }
                else {
                    count = await ctx.Database.ExecuteSqlInterpolatedAsync($@"insert into T_Papers_Teams(PaperId, TeamId, Deadline, State) values({request.PaperId}, {request.TeamId}, {examDeadline}, '1')");
                }
                return count > 0 ? new ApiResponse(200, "发布成功") : new ApiResponse(500, "发布失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> UpdateStateAsync(long id)
        {
            try
            {
                if (id == 0) return new ApiResponse(400, "试卷编号不能为空");
                Paper paper = await ctx.Papers.SingleAsync(t => t.Id == id);
                paper.State = paper.State == "1" ? "0" : "1";
                ctx.Papers.Update(paper);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "更改成功") : new ApiResponse(500, "更改失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 增加题目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseReponse> AddNewAsync(AddPaperRequest request)
        {
            try
            {
                string title = request.Title;
                long categoryId = request.CategoryId;
                List<TopicsSet> topicsSets = request.TopicsSets;
                if (string.IsNullOrEmpty(title)) return new ApiResponse(400, "试卷标题不能为空");
                if (categoryId == 0) return new ApiResponse(400, "试卷类目不能为空");
                if (topicsSets.Count == 0) return new ApiResponse(400, "试卷题目不能为空");
                Paper paper = new Paper();
                paper.Title = title;
                Category category = await ctx.Categories.SingleAsync(c => c.Id == categoryId);
                paper.Category = category;
                paper.User = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                paper.UpdateTime = DateTime.Now;
                paper.CreateTime = DateTime.Now;
                // 计算总分
                double score = 0;
                // 添加到试卷表
                List<PaperTopic> paperTopics = new List<PaperTopic>();
                foreach (TopicsSet topicsSet in topicsSets)
                {
                    long topicIndex = 1;
                    foreach (Topic topic in topicsSet.Topics)
                    {
                        PaperTopic paperTopic = new PaperTopic();
                        // paperTopic.Paper = paper;
                        // paperTopic.Topic = topic;
                        paperTopic.TopicId = topic.Id;
                        paperTopic.TopicSetIndex = topicsSet.Id;
                        paperTopic.TopicIndex = topicIndex;
                        score += topic.Score;
                        topicIndex++;
                        paperTopics.Add(paperTopic);
                    }
                }
                paper.Score = score;
                // 添加到试卷表
                await ctx.Papers.AddAsync(paper);
                await ctx.SaveChangesAsync();
                // 添加到关系表
                // await ctx.PaperTopics.AddRangeAsync(paperTopics);
                long paperId = paper.Id;
                foreach (PaperTopic item in paperTopics) {
                    await ctx.Database.ExecuteSqlInterpolatedAsync(@$"insert into T_Papers_Topics(PaperId, TopicId, TopicIndex, TopicSetIndex) values({paperId}, {item.TopicId}, {item.TopicIndex}, {item.TopicSetIndex})");
                }
                return new ApiResponse(200, "添加成功");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }


        /// <summary>
        /// 获取试卷总数
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> GetCountAsync()
        {
            try
            {
                long count = await ctx.Papers.LongCountAsync();
                return new ApiResponse(200, "获取总数成功", count);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
