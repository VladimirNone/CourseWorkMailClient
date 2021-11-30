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
        public static byte[] GetHash(byte[] data)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(data);

            return hash;
        }

        public static bool CheckHash(byte[] oldHash, byte[] data)
        {
            var newHash = GetHash(data);

            for (int i = 0; i < oldHash.Length; i++)
                if (oldHash[i] != newHash[i])
                    return false;

            return true;
        }
    }
}
