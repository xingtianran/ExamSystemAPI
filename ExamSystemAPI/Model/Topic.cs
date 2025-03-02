namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 题目模型
    /// </summary>
    public class Topic { 
        public long Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Answer { get; set; }
        // 0单选题 1多选题 2判断题 3填空题 4问答题
        public required string Type { get; set; }
        public double Score { get; set; }
        public required Category Category { get; set; }
        public required User User { get; set; }
        public List<Paper> Papers { get; set; } = new List<Paper>();
        public required string State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
