namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 类目模型
    /// </summary>
    public class Category { 
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public Category? Parent { get; set; }
        public string Name { get; set; } = string.Empty;
        public User User { get; set; } = new User();
        public List<Category> Children { get; set; } = new List<Category>();
        public string State { get; set; } = "1";
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}
