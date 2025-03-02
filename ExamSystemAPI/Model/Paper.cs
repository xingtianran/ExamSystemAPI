namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 试卷模型
    /// </summary>
    public class Paper { 
        public long Id { get; set; }
        public required string Title { get; set; }
        public double Score { get; set; }
        public required User User { get; set; }
        public List<Topic> Topics { get; set; } = new List<Topic>();
        public required string State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
