namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 图片模型
    /// </summary>
    public class Image {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OriginalName { get; set; } = string.Empty;
        // 1 avatar 2 topic
        public string Origin { get; set; } = string.Empty;
        public User User { get; set; } = new User();
        public string State { get; set; } = "1";
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
