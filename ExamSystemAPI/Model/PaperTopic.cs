namespace ExamSystemAPI.Model
{
    public class PaperTopic
    {
        public long PaperId { get; set; }
        public Paper Paper { get; set; } = new Paper();
        public long TopicId { get; set; }
        public Topic Topic { get; set; } = new Topic();
        public long TopicSetIndex { get; set; }
        public long TopicIndex { get; set; }
    }
}
