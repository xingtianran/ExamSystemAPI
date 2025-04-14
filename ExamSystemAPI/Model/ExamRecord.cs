namespace ExamSystemAPI.Model
{
    public class ExamRecord
    {
        public long Id { get; set; }
        public User User { get; set; } = new User();
        public string Name { get; set; } = string.Empty;
        public double Score { get; set; }
        public string State { get; set; } = "1";
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
