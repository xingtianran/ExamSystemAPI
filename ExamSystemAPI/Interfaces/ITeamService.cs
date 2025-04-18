﻿using ExamSystemAPI.Extensions.Request;
using ExamSystemAPI.Extensions.Response;
using ExamSystemAPI.Model;

namespace ExamSystemAPI.Interfaces
{
    public interface ITeamService
    {
        Task<BaseReponse> AddNewAsync(Team team);
        Task<BaseReponse> GetSingleAsync(long id);
        Task<BaseReponse> GetAllAsync(QueryTeamsParametersRequest request);
        Task<BaseReponse> GetAllAsync(QueryParametersRequest request);
        Task<BaseReponse> DeleteAsync(long id);
        Task<BaseReponse> UpdateAsync(Team team);
        Task<BaseReponse> UpdateStateAsync(long id);
        Task<BaseReponse> GetCountAsync();
        Task<BaseReponse> GetListAsync();
        Task<BaseReponse> GetMyAsync();
    }
}
