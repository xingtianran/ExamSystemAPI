using Microsoft.AspNetCore.Identity;

namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public class User : IdentityUser<long> {
        public long JWTVersion { get; set; }
        public List<Team> Teams { get; set; } = new List<Team>();
    }
}
