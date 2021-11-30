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

        public void CreateNewKey(string keyName)
        {
            RsaService = new RSACryptoServiceProvider(2048);
            //KeyContainer.WriteKeyToConteiner(RsaService.ToXmlString(true), keyName, isPrivate: true, isSignature: false);
        }

        public void SavePublicKey(string keyName)
        {
            if (RsaService == null)
                throw new Exception("Публичный ключ отсутсвует");

            //KeyContainer.WriteKeyToConteiner(RsaService.ToXmlString(false), keyName, isPrivate: false, isSignature: false);
        }

        public void LoadKey(string keyName, bool isPrivate)
        {
            RsaService = new RSACryptoServiceProvider(2048);
            try
            {
                //RsaService.FromXmlString(KeyContainer.ReadKeyFromContainer(keyName, isPrivate, isSignature: false));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        public static byte[] EncryptUsingDes(byte[] encryptedData, string nameOfKey)
        {
            using MemoryStream memoryStream = new(encryptedData);

            using DES des = DES.Create();

            des.GenerateKey();

            var iv = des.IV;
            var ivLength = BitConverter.GetBytes(iv.Length);

            string SymKey = Convert.ToBase64String(des.Key);

            KeyContainer.WriteKeyToConteiner(SymKey, nameOfKey);

            using CryptoStream cryptoStream = new(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Read);
            using var memoryStream2 = new MemoryStream();
            cryptoStream.CopyTo(memoryStream2);

            return memoryStream2.ToArray().Concat(iv).Concat(ivLength).ToArray();
        }

        public static byte[] DecryptUsingDes(byte[] encryptedData, string nameOfKey)
        {
            var ivLength = BitConverter.ToInt32(encryptedData[^4..]);

            var iv = encryptedData[^(ivLength + 4)..^4];

            var SymKey = Convert.FromBase64String(KeyContainer.ReadKeyFromContainer(nameOfKey));

            using MemoryStream memoryStream = new(encryptedData[..^(ivLength + 4)]);
            using DES des = DES.Create();
            using CryptoStream cryptoStream = new(memoryStream, des.CreateDecryptor(SymKey, iv), CryptoStreamMode.Read);
            MemoryStream memoryStream2 = new MemoryStream();
            cryptoStream.CopyTo(memoryStream2);

            return memoryStream2.ToArray();
        }
    }
}
