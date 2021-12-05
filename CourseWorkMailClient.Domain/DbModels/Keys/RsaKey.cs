using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Domain.Keys
{
    public class RsaKey : Entity
    {
        public string MessageId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public DateTime DeathTime { get; set; }

        public List<Interlocutor> Interlocutors { get; set; }
    }
}
