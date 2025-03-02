namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 图片模型
    /// </summary>
    public class Image { 
        public long Id { get; set; }
        public required string Url { get; set; }
        public required string Name { get; set; }
        public required string Path { get; set; }
        public required string ContentType { get; set; }
        public required User User { get; set; }
        public required string State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
