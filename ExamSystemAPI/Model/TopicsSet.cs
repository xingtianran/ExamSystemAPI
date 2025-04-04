namespace ExamSystemAPI.Model
{
    public class TopicsSet
    {
        public long Id { get; set; }
        public List<Topic> Topics { get; set; } = new List<Topic>();
    }
}
