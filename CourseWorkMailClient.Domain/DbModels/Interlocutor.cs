using CourseWorkMailClient.Domain.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Domain
{
    public class Interlocutor : Entity
    {
        public string Email { get; set; }

        public int? LastMD5RsaKeyId { get; set; }
        public MD5RsaKey LastMD5RsaKey { get; set; }

        public int? LastDESRsaKeyId { get; set; }
        public DESRsaKey LastDESRsaKey { get; set; }

        public List<Letter> SendedLetters { get; set; }
        public List<Letter> ReceivedLetters { get; set; }
    }
}
