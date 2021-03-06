using CourseWorkMailClient.Domain.Keys;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CourseWorkMailClient.Domain
{
    public class Letter : Entity, INotifyPropertyChanged
    {
        public int UniqueId { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get => Date.ToString(); }
        public string Subject { get; set; }
        [NotMapped]
        private bool seen;
        public bool Seen
        {
            get => seen;
            set
            {
                seen = value;
                OnPropertyChanged();
            }
        }


        [NotMapped]
        public string Content { get; set; }

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

        public int FolderId { get; set; }
        public Folder Folder { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
