﻿namespace ExamSystemAPI.Extensions.Request
{
    public class PublishPaperRequest
    {
        public long PaperId { get; set; }
        public long TeamId { get; set; }
        public long Deadline { get; set; }
    }
}
