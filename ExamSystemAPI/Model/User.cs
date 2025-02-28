namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 用户模型
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Password"></param>
    /// <param name="Email"></param>
    /// <param name="Phone"></param>
    /// <param name="Role">role_admin role_normal</param>
    /// <param name="State">0禁用 1正常</param>
    /// <param name="CreateTime"></param>
    /// <param name="UpdateTime"></param>
    public record User(long Id, string Name, string Password, string Email, string Phone, string Role, string State, DateTime CreateTime, DateTime UpdateTime);
}
