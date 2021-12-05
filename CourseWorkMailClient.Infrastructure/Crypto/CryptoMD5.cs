using CourseWorkMailClient.Domain.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class CryptoMD5
    {
        private RSACryptoServiceProvider Rsa;

        public void CreateNewRsaKey()
        {
            Rsa = new RSACryptoServiceProvider(2048);
        }

        public MD5RsaKey GetRsaKey()
        {
            var key = new MD5RsaKey() { DeathTime = DateTime.Now.AddDays(14) };

            key.PublicKey = Rsa.ToXmlString(false);

            if (!Rsa.PublicOnly)
            {
                key.PrivateKey = Rsa.ToXmlString(true);
            }

            return key;
        }

        public void SetRsaKey(string key)
        {
            Rsa.FromXmlString(key);
        }

        public byte[] GetHash(byte[] data)
        {
            return Rsa.SignData(data, MD5.Create());
        }

        public bool CheckHash(byte[] oldHash, byte[] data)
        {
            return Rsa.VerifyData(data, MD5.Create(), oldHash);
        }
    }
}
