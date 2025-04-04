namespace ExamSystemAPI.Extensions.Request
{
    public class QueryPapersParametersRequest : QueryParametersRequest
    {
        public string? Title { get; set; }
        public long CategoryId { get; set; }
    }
}
