namespace ExamSystemAPI.Extensions.Request
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        // 1 student 2 teacher
        public string Role { get; set; }
    }
}
