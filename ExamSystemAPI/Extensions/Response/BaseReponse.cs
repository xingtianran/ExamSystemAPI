namespace ExamSystemAPI.Extensions.Response
{
    public class BaseReponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public BaseReponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
