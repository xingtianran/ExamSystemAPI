using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Model;

namespace ExamSystemAPI.Interfaces
{
    public interface ITopicService
    {
        Task<BaseReponse> AddNewAsync(Topic topic);
        Task<BaseReponse> GetSingleAsync(long id);
        Task<BaseReponse> GetAllAsync(QueryTopicsParametersRequest request);
        Task<BaseReponse> DeleteAsync(long id);
        Task<BaseReponse> UpdateAsync(Topic topic);
        Task<ApiResponse> AddTopic2PaperAsync(AddTopic2PaperRequest request);
        Task<BaseReponse> UpdateStateAsync(long id);
        Task<BaseReponse> GetPartAsync(string ids);
        Task<BaseReponse> GetCountAsync();
        Task<BaseReponse> GetNewAsync(int size);
    }
}
