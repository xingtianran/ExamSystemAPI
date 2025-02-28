namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 类目模型
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Parent"></param>
    /// <param name="Name"></param>
    /// <param name="User">关联用户</param>
    /// <param name="State">0禁用 1正常</param>
    /// <param name="CreateTime"></param>
    /// <param name="UpdateTime"></param>
    public record Category(long Id, Category Parent, string Name, User User, string State, DateTime CreateTime, DateTime UpdateTime);
}
