namespace ExamSystemAPI.Extensions.Request
{
    public class MarkPaperRequest
    {
        public long Id { get; set; }
        public List<MarkTopic> Topics { get; set; } = new List<MarkTopic>();
    }

    public class MarkTopic { 
        public long Id { get; set; }
        public string Answer { get; set; } = string.Empty;
    }
}
