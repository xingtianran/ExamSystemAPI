namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 题目模型
    /// </summary>
    public class Topic { 
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        // 0单选题 1多选题 2判断题 3填空题 4问答题
        public string Type { get; set; } = string.Empty;
        public double Score { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; } = new Category();
        public User User { get; set; } = new User();
        public List<Paper> Papers { get; set; } = new List<Paper>();
        public string State { get; set; } = "1";
        public DateTime TempTime { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
