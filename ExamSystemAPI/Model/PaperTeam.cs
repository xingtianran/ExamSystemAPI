namespace ExamSystemAPI.Model
{
    public class PaperTeam
    {
        public long PaperId { get; set; }
        public Paper Paper { get; set; } = new Paper();
        public long TeamId { get; set; }
        public Team Team { get; set; } = new Team();
        public string State { get; set; } = "1";
        public DateTime Deadline { get; set; }
    }
}
