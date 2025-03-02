namespace ExamSystemAPI.Extensions
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public ApiResponse(int code, string message) {
            Code = code;
            Message = message;
        }
        public ApiResponse(int code, string message, object data) {
            Code = code;
            Message = message;
            Data = data;
        }
    }
}
