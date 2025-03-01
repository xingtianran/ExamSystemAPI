﻿namespace ExamSystemAPI.Model
{
    /// <summary>
    /// 图片模型
    /// </summary>
    public class Image { 
        public long Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
        public User User { get; set; }
        public string State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
