using ExamSystemAPI.Extensions;
using ExamSystemAPI.Extensions.Request;

namespace ExamSystemAPI.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse> Init();
        Task<ApiResponse> Register(RegisterRequest request);
    }
}
