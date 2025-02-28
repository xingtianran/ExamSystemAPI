namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 题目模型
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Ttile"></param>
    /// <param name="Content"></param>
    /// <param name="Answer"></param>
    /// <param name="Type">0单选题 1多选题 2判断题 3填空题 4问答题</param>
    /// <param name="Score"></param>
    /// <param name="Category">关联类目</param>
    /// <param name="User">关联用户</param>
    /// <param name="State">0禁用 1正常</param>
    /// <param name="CreateTime"></param>
    /// <param name="UpdateTime"></param>
    public record Topic(long Id, string Ttile, string Content, string Answer, string Type, double Score, Category Category, User User, string State, DateTime CreateTime, DateTime UpdateTime);
}
