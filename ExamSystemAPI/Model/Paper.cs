﻿namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 试卷模型
    /// </summary>
    public class Paper {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Score { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; } = new Category();
        public User User { get; set; } = new User();
        public List<Topic> Topics { get; set; } = new List<Topic>();
        public List<Team> Teams { get; set; } = new List<Team>();
        public string State { get; set; } = "1";
        public long TempId { get; set; }
        public DateTime TempTime { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
