using ExamSystemAPI.Extensions.Response;

namespace ExamSystemAPI.Interfaces
{
    public interface IOssService
    {
        Task<BaseReponse> UploadFileAsync(IFormFile file, string origin);
        Task<Stream> DownloadFileAsync(string fileName);
    }
}
