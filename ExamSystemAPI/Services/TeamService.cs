using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Interfaces;
using ExamSystemAPI.Model;

namespace ExamSystemAPI.Services
{
    public class TeamService : ITeamService
    {
        public Task<BaseReponse> AddNewAsync(Team team)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReponse> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReponse> GetAllAsync(QueryParametersRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReponse> GetSingleAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseReponse> UpdateAsync(Team team)
        {
            throw new NotImplementedException();
        }
    }
}
