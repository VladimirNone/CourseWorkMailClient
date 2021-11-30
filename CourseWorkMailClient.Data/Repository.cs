using CourseWorkMailClient.Domain.Keys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Data
{
    public class Repository
    {
        private KeyDbContext db { get; set; }

        public Repository()
        {
            db = new KeyDbContext();
        }

        public bool TryGetRsaKeys(string emailOfInterlocutor, out DESRsaKey desRsaKey, out MD5RsaKey md5RsaKey)
        {
            var interlocutor = db.Interlocutors.Include(h => h.LastMD5RsaKey).Include(h => h.LastDESRsaKey).FirstOrDefault(h => h.Email == emailOfInterlocutor);

            if (interlocutor == null)
            {
                desRsaKey = null;
                md5RsaKey = null;

                return false;
            }

            desRsaKey = interlocutor.LastDESRsaKey;
            md5RsaKey = interlocutor.LastMD5RsaKey;

            return true;
        }

        public void AddRsaKeys(string emailOfInterlocutor, DESRsaKey desRsaKey, MD5RsaKey md5RsaKey)
        {
            var interlocutor = db.Interlocutors.Include(h => h.LastMD5RsaKey).Include(h => h.LastDESRsaKey).FirstOrDefault(h => h.Email == emailOfInterlocutor);
        }
    }
}
