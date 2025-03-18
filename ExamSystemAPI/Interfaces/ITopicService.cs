using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Model;

namespace ExamSystemAPI.Interfaces
{
    public interface ITopicService
    {
        Task<BaseReponse> AddNewAsync(Topic topic, string sign);
        Task<BaseReponse> GetSingleAsync(long id);
        Task<BaseReponse> GetAllAsync(QueryParametersRequest request);
        Task<BaseReponse> DeleteAsync(long id);
        Task<BaseReponse> UpdateAsync(Topic topic);

    }
}
