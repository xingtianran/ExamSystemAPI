namespace ExamSystemAPI.Extensions
{
    public class JWTSettings
    {
        public required string SecKey { get; set; }
        public int ExpireSeconds { get; set; }
    }
}
