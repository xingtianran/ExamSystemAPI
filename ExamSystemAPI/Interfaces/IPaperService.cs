using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Model;

namespace ExamSystemAPI.Interfaces
{
    public interface IPaperService
    {
        Task<BaseReponse> AddNewAsync(Paper paper, string sign);
        Task<BaseReponse> GetSingleAsync(long id);
        Task<BaseReponse> GetAllAsync(QueryParametersRequest request);
        Task<BaseReponse> DeleteAsync(long id);
        Task<BaseReponse> UpdateAsync(Paper paper);
        Task<BaseReponse> PublishAsync(PublishPaperRequest request);
    }
}
