using ExamSystemAPI.Model;

namespace ExamSystemAPI.Extensions.Request
{
    public class AddPaperRequest
    {
        public string Title { get; set; } = string.Empty;
        public long CategoryId { get; set; }
        public List<TopicsSet> TopicsSets { get; set; } = new List<TopicsSet>();
    }
}
