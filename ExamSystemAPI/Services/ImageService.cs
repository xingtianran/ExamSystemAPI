using Aliyun.OSS;
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
    public class ImageService : IImageService
    {
        private readonly MyDbContext ctx;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly OssClient ossClient;
        private readonly string bucketName;

        public ImageService(MyDbContext ctx, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, OssClient ossClient, IConfiguration configuration)
        {
            this.ctx = ctx;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.ossClient = ossClient;
            this.bucketName = configuration["AliyunOSS:BucketName"]!;
        }

        /// <summary>
        /// 增加照片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public async Task<BaseReponse> AddNewAsync(string fileName, string newName, string origin)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName)) return new ApiResponse(400, "图片名称不能为空");
                if (string.IsNullOrEmpty(origin)) return new ApiResponse(400, "图片来源不能为空");
                Image image = new Image();
                image.Name = newName;
                image.OriginalName = fileName;
                image.Origin = origin;
                image.User = (await userManager.FindByIdAsync(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
                image.CreateTime = DateTime.Now;
                image.UpdateTime = DateTime.Now;
                await ctx.Images.AddAsync(image);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "保存成功") : new ApiResponse(500, "保存失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }


        /// <summary>
        /// 获取全部图片
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseReponse> GetAllAsync(QueryImagesParametersRequest request)
        {
            try
            {
                int page = request.Page;
                int size = request.Size;
                string? origin = request.Origin;
                int startIndex = (page - 1) * size;
                IQueryable<Image> baseSet = ctx.Images.Include(i => i.User);
                if (!string.IsNullOrEmpty(origin))
                    baseSet = baseSet.Where(b => b.Origin == origin);
                var data = await baseSet.Skip(startIndex).Take(size).ToListAsync();
                var count = await baseSet.CountAsync();
                var totalPage = (int)Math.Ceiling(count * 1.0 / size);
                return new PageInfoResponse<Image>(200, "获取成功", page, size, totalPage, data);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }


        /// <summary>
        /// 更新状态
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReponse> UpdateStateAsync(long id)
        {
            try
            {
                if (id == 0) return new ApiResponse(400, "图片编号不能为空");
                Image image = await ctx.Images.SingleAsync(i => i.Id == id);
                // 更改状态
                image.State = image.State == "1" ? "0" : "1";
                ctx.Images.Update(image);
                return await ctx.SaveChangesAsync() > 0 ? new ApiResponse(200, "修改成功") : new ApiResponse(500, "修改失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseReponse> DeleteAsync(long id)
        {
            try
            {
                if (id == 0) return new ApiResponse(400, "图片编号不能为空");
                Image image = await ctx.Images.SingleAsync(i => i.Id == id);
                ctx.Images.Remove(image);
                // 删除oss中的图片
                var ossResult = ossClient.DeleteObject(bucketName, image.Name);
                return await ctx.SaveChangesAsync() > 0 && ((int)ossResult.HttpStatusCode >= 200 && (int)ossResult.HttpStatusCode < 300) ? new ApiResponse(200, "删除成功") : new ApiResponse(500, "删除失败");
            }
            catch (Exception ex) {
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
