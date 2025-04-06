using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystemAPI.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService imageService;

        public ImagesController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        /// <summary>
        /// 获取全部图片
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<BaseReponse> GetAll([FromQuery] QueryImagesParametersRequest request) => imageService.GetAllAsync(request);


        /// <summary>
        /// 更新状态
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Task<BaseReponse> UpdateState(long id) => imageService.UpdateStateAsync(id);

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete("{id}")]
        public Task<BaseReponse> Delete(long id) => imageService.DeleteAsync(id);
    }
}
