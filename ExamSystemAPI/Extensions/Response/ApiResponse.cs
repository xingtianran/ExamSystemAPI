namespace ExamSystemAPI.Extensions.Response
{
    public class ApiResponse : BaseReponse
    {
        public object? Data { get; set; }

        public ApiResponse(int code, string message) : base(code, message)
        {
        }
        public ApiResponse(int code, string message, object? data) : base(code, message)
        {
            Data = data;
        }
    }
}
