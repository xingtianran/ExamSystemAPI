using Aliyun.OSS;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Helper;
using ExamSystemAPI.Interfaces;

namespace ExamSystemAPI.Services
{
    public class OssService : IOssService
    {
        private readonly OssClient ossClient;
        private readonly string bucketName;
        private readonly IImageService imageService;

        public OssService(OssClient ossClient, IConfiguration configuration, IImageService imageService)
        {
            this.ossClient = ossClient;
            this.bucketName = configuration["AliyunOSS:BucketName"]!;
            this.imageService = imageService;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public async Task<BaseReponse> UploadFileAsync(IFormFile file, string origin)
        {
            try
            {
                string fileName = file.FileName;
                string newName = FileHelper.GenerateFileName(fileName);
                using (var stream = file.OpenReadStream())
                {
                    var putObjectRequest = new PutObjectRequest(bucketName, newName, stream);
                    var result = ossClient.PutObject(putObjectRequest);
                    if ((int)result.HttpStatusCode < 200 && (int)result.HttpStatusCode >= 300) {
                        return new ApiResponse((int)result.HttpStatusCode, "上传失败");
                    }
                    // 保存到图片记录表
                    await imageService.AddNewAsync(fileName, newName, origin);
                    return new ApiResponse(200, "上传成功");
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            try
            {
                var getObjectRequest = new GetObjectRequest(bucketName, fileName);
                var ossObject = ossClient.GetObject(getObjectRequest);
                var ms = new MemoryStream();
                await ossObject.Content.CopyToAsync(ms);
                ms.Position = 0;
                return ms;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
