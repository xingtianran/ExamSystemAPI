namespace ExamSystemAPI.Extensions.Response
{
    public class UserInfoResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
