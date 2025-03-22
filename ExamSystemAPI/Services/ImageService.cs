using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;
using ExamSystemAPI.Model.DbContexts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ExamSystemAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly MyDbContext ctx;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        public ImageService(MyDbContext ctx, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            this.ctx = ctx;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
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
    }
}
