namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public class User {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        // role_admin role_normal
        public string Role { get; set; }
        public string State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
