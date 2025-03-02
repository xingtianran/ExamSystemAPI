namespace ExamSystemAPI.Extensions.Request
{
    public class RegisterRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        // 1 student 2 teacher
        public required string Role { get; set; }
    }
}
