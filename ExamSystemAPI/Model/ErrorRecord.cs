namespace ExamSystemAPI.Model
{
    public class ErrorRecord
    {
        public long Id { get; set; }
        public User User { get; set; } = new User();
        public Topic Topic { get; set; } = new Topic();
        public string Answer { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
