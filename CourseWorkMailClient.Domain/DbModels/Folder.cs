using MailKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Domain
{
    public class Folder : Entity
    {
        public string Title { get; set; }
        public int MailServerId { get; set; }
        public int? CountOfMessage { get; set; }
        [NotMapped]
        public MailFolder Source { get; set; }

        public List<Letter> Letters { get; set; }
    }
}
