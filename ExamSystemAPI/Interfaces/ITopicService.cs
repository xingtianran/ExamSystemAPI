using ExamSystemAPI.Extensions;
using ExamSystemAPI.Model;

namespace ExamSystemAPI.Interfaces
{
    public interface ITopicService
    {
        Task<ApiResponse> AddNew(Topic topic);
    }
}
