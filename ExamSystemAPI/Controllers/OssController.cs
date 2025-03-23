using Aliyun.OSS;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Helper.Filter;
using ExamSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystemAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OssController : ControllerBase
    {
        private readonly IOssService ossService;

        public OssController(IOssService ossService)
        {
            this.ossService = ossService;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [NotCheckJWTValiadation]
        public Task<BaseReponse> UploadFile(IFormFile file, string origin) => ossService.UploadFileAsync(file, origin);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("{fileName}")]
        [NotCheckJWTValiadation]
        public async  Task<IActionResult> DownloadFile(string fileName) {
            try
            {
                var fileStream = await ossService.DownloadFileAsync(fileName);
                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
