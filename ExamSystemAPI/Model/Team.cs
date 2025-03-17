namespace ExamSystemAPI.Model
{
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Count { get; set; }
        public string State { get; set; } = "1";
        public List<Paper> Papers { get; set; } = new List<Paper>();
        public List<User> Users { get; set; } = new List<User>();
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
