using CourseWorkMailClient.Domain.Keys;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWorkMailClient.Domain
{
    public class Letter : Entity
    {
        public string MessageId { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        [NotMapped]
        public string Content { get; set; }
        public bool LocalMessage { get; set; }

        public string From { get; set; }
        public string To { get; set; }

        public List<Interlocutor> Senders { get; set; }
        public List<Interlocutor> Receivers { get; set; }
        public List<Attachment> Attachments { get; set; }

        [NotMapped]
        public MimeMessage Source { get; set; }

        public string PathToFullMessageFile { get; set; }

        public int? MD5RsaKeyId { get; set; }
        public MD5RsaKey MD5RsaKey { get; set; }

        public int? DESRsaKeyId { get; set; }
        public DESRsaKey DESRsaKey { get; set; }

        public List<Folder> Folders { get; set; }
    }
}
