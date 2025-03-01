namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 类目模型
    /// </summary>
    public class Category { 
        public long Id { get; set; }
        public Category? Parent { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public List<Category> Children { get; set; } = new List<Category>();
        public string State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}
