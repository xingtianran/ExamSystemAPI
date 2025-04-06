using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Model;

namespace ExamSystemAPI.Interfaces
{
    public interface IPaperService
    {
        Task<BaseReponse> AddNewAsync(AddPaperRequest request);
        Task<BaseReponse> GetSingleAsync(long id);
        Task<BaseReponse> GetAllAsync(QueryPapersParametersRequest request);
        Task<BaseReponse> DeleteAsync(long id);
        Task<BaseReponse> UpdateAsync(Paper paper);
        Task<BaseReponse> PublishAsync(PublishPaperRequest request);
        Task<BaseReponse> UpdateStateAsync(long id);
        Task<BaseReponse> GetCountAsync();
    }
}
