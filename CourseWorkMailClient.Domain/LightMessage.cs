﻿using MimeKit;
using System;
using System.Collections.Generic;

namespace CourseWorkMailClient.Domain
{
    public class LightMessage
    {
        public uint Id { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool LocalMessage { get; set; }
        public string From { get; set; }
        public List<string> Froms { get; set; }
        public List<string> To { get; set; }
        public List<string> Attachments { get; set; }
        public MimeMessage Source { get; set; }
    }
}