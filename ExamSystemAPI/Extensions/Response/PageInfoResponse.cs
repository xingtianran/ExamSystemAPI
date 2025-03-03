namespace ExamSystemAPI.Extensions.Response
{
    public class PageInfoResponse<T> : BaseReponse
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalPage { get; set; }
        public List<T> Data { get; set; }

        public PageInfoResponse(int code, string message, int page, int size, int totalPage, List<T> data) : base(code, message)
        {
            Page = page;
            Size = size;
            TotalPage = totalPage;
            Data = data;
        }
    }
}
