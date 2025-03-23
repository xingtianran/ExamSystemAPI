namespace ExamSystemAPI.Extensions.Request
{
    public class QueryUsersParametersRequest : QueryParametersRequest
    {
        public string? UserName { get; set; }
        public string? Role { get; set; }
    }
}
