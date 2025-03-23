namespace ExamSystemAPI.Extensions.Response
{
    public class UserInfoResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        // 0 为锁定 1 为不锁定
        public string IsLock { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
