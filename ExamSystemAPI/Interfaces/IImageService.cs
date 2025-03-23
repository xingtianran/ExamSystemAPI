using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;

namespace ExamSystemAPI.Interfaces
{
    public interface IImageService
    {
        Task<BaseReponse> AddNewAsync(string fileName, string newName, string origin);
        Task<BaseReponse> DeleteAsync(long id);
        Task<BaseReponse> GetAllAsync(QueryImagesParametersRequest request);
        Task<BaseReponse> UpdateStateAsync(long id);
    }
}
