using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;

namespace ExamSystemAPI.Interfaces
{
    public interface IUserService
    {
        Task<BaseReponse> InitAsync();
        Task<BaseReponse> RegisterAsync(RegisterRequest request);
        Task<BaseReponse> LoginAsync(string username, string password);
        Task<BaseReponse> LogoutAsync();
    }
}
