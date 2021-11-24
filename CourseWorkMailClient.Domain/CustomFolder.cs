using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Domain
{
    public class CustomFolder
    {
        public string Title { get; set; }
        public int? CountOfMessage { get; set; }
        public MailFolder Source { get; set; }
    }
}
