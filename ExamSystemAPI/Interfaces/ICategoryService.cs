using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Model;

namespace ExamSystemAPI.Interfaces
{
    public interface ICategoryService
    {
        Task<BaseReponse> AddNewAsync(Category category);
        Task<BaseReponse> GetSigleAsync(long id);
        Task<BaseReponse> GetAllAsync(QueryParametersRequest request);
        Task<BaseReponse> UpdateAsync(Category category);
    }
}
