using CourseWorkMailClient.Domain.Keys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class CryptoDES
    {
        private RSACryptoServiceProvider RsaService { get; set; }

        public void CreateNewRsaKey()
        {
            RsaService = new RSACryptoServiceProvider(2048);
        }

        public byte[] EncryptUsingRsa(byte[] data)
        {
            var ecnrypted = RsaService.Encrypt(data, RSAEncryptionPadding.Pkcs1);

            return ecnrypted;
        }

        public byte[] DecryptUsingRsa(byte[] data)
        {
            try
            {
                return RsaService.Decrypt(data, RSAEncryptionPadding.Pkcs1);
            }
            catch
            {
                throw new Exception("Расшифровка невозможна. Возможные причины: поврежден ключ, неверный ключ, использован публичный ключ");
            }
        }

        public DESRsaKey GetRsaKey()
        {
            var key = new DESRsaKey() { DeathTime = DateTime.Now.AddDays(14) };

            key.PublicKey = RsaService.ToXmlString(false);

            if (!RsaService.PublicOnly)
            {
                key.PrivateKey = RsaService.ToXmlString(true);
            }

            return key;
        }

        public void SetRsaKey(string key)
        {
            RsaService.FromXmlString(key);
        }

        public byte[] EncryptUsingDes(byte[] content)
        {
            using var fileStream = new MemoryStream(content);

            using DES des = DES.Create();

            des.GenerateKey();

            var iv = des.IV;
            var ivLength = BitConverter.GetBytes(iv.Length);

            var encryptedSymKey = EncryptUsingRsa(des.Key);
            var encryptedSymKeyLength = BitConverter.GetBytes(encryptedSymKey.Length);

            using CryptoStream cryptoStream = new(fileStream, des.CreateEncryptor(), CryptoStreamMode.Read);
            using var memoryStream2 = new MemoryStream();
            cryptoStream.CopyTo(memoryStream2);

            return memoryStream2.ToArray().Concat(iv).Concat(ivLength).Concat(encryptedSymKey).Concat(encryptedSymKeyLength).ToArray();
        }

        public byte[] DecryptUsingDes(byte[] encryptedData)
        {
            var encryptedSymKeyLength = BitConverter.ToInt32(encryptedData[^4..]);

            var encryptedSymKey = encryptedData[^(encryptedSymKeyLength + 4)..^4];

            var ivLength = BitConverter.ToInt32(encryptedData[^(encryptedSymKeyLength + 4 + 4)..^(encryptedSymKeyLength + 4)]);

            var iv = encryptedData[^(encryptedSymKeyLength + ivLength + 4 + 4)..^(encryptedSymKeyLength + 4 + 4)];

            var SymKey = DecryptUsingRsa(encryptedSymKey);

            using MemoryStream memoryStream = new(encryptedData[..^(encryptedSymKeyLength + ivLength + 4 + 4)]);
            using DES des = DES.Create();
            using CryptoStream cryptoStream = new(memoryStream, des.CreateDecryptor(SymKey, iv), CryptoStreamMode.Read);
            MemoryStream memoryStream2 = new MemoryStream();
            cryptoStream.CopyTo(memoryStream2);

            return memoryStream2.ToArray();
        }
    }
}
