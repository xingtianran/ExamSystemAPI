using ExamSystemAPI.Extensions.Response;

namespace ExamSystemAPI.Interfaces
{
    public interface IImageService
    {
        Task<BaseReponse> AddNewAsync(string fileName, string newName, string origin);
    }
}
