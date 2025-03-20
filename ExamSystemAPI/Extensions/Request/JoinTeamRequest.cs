namespace ExamSystemAPI.Extensions.Request
{
    public class JoinTeamRequest
    {
        public long TeamId { get; set; }
        public long UserId { get; set; }
        public string Password { get; set; }
    }
}
