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
        Task<BaseReponse> GetAllAsync(QueryUsersParametersRequest request);
        Task<BaseReponse> JoinTeamAsync(JoinTeamRequest request);
        Task<BaseReponse> CheckStatusAsync();
        Task<BaseReponse> GetRolesAsync();
        Task<BaseReponse> ResetPwdAsync(long userId, string newPwd);
        Task<BaseReponse> LockUserAsync(long userId);
        Task<BaseReponse> UnLockUser(long userId);
        Task<BaseReponse> GetCountAsync();
        Task<BaseReponse> GetMyExam();
        Task<BaseReponse> GetExamDetail(long paperId, long teamId);
        Task<BaseReponse> MarkPaper(MarkPaperRequest request);
    }
}
