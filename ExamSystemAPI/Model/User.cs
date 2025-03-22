using Microsoft.AspNetCore.Identity;

namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public class User : IdentityUser<long> {
        public long JWTVersion { get; set; }
        public string Avatar { get; set; } = "5e5153cd-e9a3-450d-9070-3c3aedb87f6c.jpg";
        public List<Team> Teams { get; set; } = new List<Team>();
    }
}
