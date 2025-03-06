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
    public class CategoryService : ICategoryService
    {
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly MyDbContext ctx;
        public CategoryService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, MyDbContext ctx)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.ctx = ctx;
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<BaseReponse> AddNewAsync(Category category)
        {
            try
            {
                if (string.IsNullOrEmpty(category.Name))
                    return new ApiResponse(400, "类目名称不能为空");
                User user = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                category.User = user;
                category.CreateTime = DateTime.Now;
                category.UpdateTime = DateTime.Now;
                if (category.ParentId != null && category.ParentId != 0) {
                    Category parent = await ctx.Categories.FirstAsync(c => c.Id == category.ParentId);
                    if (parent == null)
                        return new ApiResponse(400, "父结点信息无效");
                }
                await ctx.Categories.AddAsync(category);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "添加成功") : new ApiResponse(500, "添加失败");
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取全部分类
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
                var baseSet = ctx.Categories.Where(c => c.Parent == null);
                var data = await baseSet.Skip(startIndex).Take(size).ToListAsync();
                // 树型格式取出全部元素
                foreach (var item in data) {
                    await SearchChilrenAsync(item.Id);
                }
                // 每一个根结点为一条
                int count = await baseSet.CountAsync();
                int totalPage = (int)Math.Ceiling(count * 1.0 / size);
                return new PageInfoResponse<Category>(200, "获取成功", page, size, totalPage, data);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 遍历寻找每一结点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task SearchChilrenAsync(long id)
        {
            var parent = await ctx.Categories.Include(c => c.Children).FirstAsync(c => c.Id == id);
            foreach (var child in parent.Children)
            {
                // 父结点都置空，防止循环引用
                child.Parent = null;
                await SearchChilrenAsync(child.Id);
            }
        }

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetSigleAsync(long id)
        {
            try
            {
                if (id == 0)
                    return new ApiResponse(400, "分类编号不能为空");
                Category category = await ctx.Categories.FirstAsync(c => c.Id == id);
                return category != null ? new ApiResponse(200, "获取成功", category) : new ApiResponse(500, "获取失败", category);
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<BaseReponse> UpdateAsync(Category category)
        {
            try
            {
                if (category.Id == 0)
                    return new ApiResponse(400, "分类编号不能为空");
                if (string.IsNullOrEmpty(category.Name))
                    return new ApiResponse(400, "分类名称不能为空");
                Category categoryFromDB = await ctx.Categories.FirstAsync(c => c.Id == category.Id);
                categoryFromDB.Name = category.Name;
                categoryFromDB.State = category.State;
                categoryFromDB.ParentId = category.ParentId;
                categoryFromDB.User = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                categoryFromDB.UpdateTime = DateTime.Now;
                ctx.Categories.Update(categoryFromDB);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "更新成功") : new ApiResponse(500, "更新失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
