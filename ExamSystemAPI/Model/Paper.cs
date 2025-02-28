namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 试卷模型
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Title"></param>
    /// <param name="Score"></param>
    /// <param name="User">关联用户</param>
    /// <param name="State"></param>
    /// <param name="CreateTime"></param>
    /// <param name="UpdateTime"></param>
    public record Paper(long Id, string Title, double Score, User User, string State, DateTime CreateTime, DateTime UpdateTime);
}
