namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 图片模型
    /// </summary>
    public class Image { 
        public long Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public User User { get; set; } = new User();
        public string State { get; set; } = "1";
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
