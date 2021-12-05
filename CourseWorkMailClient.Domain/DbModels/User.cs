using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Domain
{
    public class User : Entity
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public int MailServerId { get; set; }
        public MailServer MailServer { get; set; }
    }
}
