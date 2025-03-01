namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 题目模型
    /// </summary>
    public class Topic { 
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Answer { get; set; }
        // 0单选题 1多选题 2判断题 3填空题 4问答题
        public string Type { get; set; }
        public double Score { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }
        public List<Paper> Papers { get; set; } = new List<Paper>();
        public string State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
