namespace ExamSystemAPI.Extensions.Request
{
    public class QueryTopicsParametersRequest : QueryParametersRequest
    {
        public string? Title { get; set; }
        public string? Type { get; set; }
        public long CategoryId { get; set; }
    }
}
