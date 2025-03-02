namespace ExamSystemAPI.Model
{
    public class Team
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public int Count { get; set; }
        public required string State { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
