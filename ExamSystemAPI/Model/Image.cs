namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 图片模型
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Url"></param>
    /// <param name="Name"></param>
    /// <param name="Path"></param>
    /// <param name="ContentType"></param>
    /// <param name="User"></param>
    /// <param name="State"></param>
    /// <param name="CreateTime"></param>
    /// <param name="UpdateTime"></param>
    public record Image(long Id, string Url, string Name, string Path, string ContentType, User User, string State, DateTime CreateTime, DateTime UpdateTime);
}
