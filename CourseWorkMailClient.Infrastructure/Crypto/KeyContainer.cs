using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public static class KeyContainer
    {
        private static string GetNameOfContainer()
            => "KeysContainer" + ".txt";

        public static void WriteKeyToConteiner(string key, string keyName)
        {
            var containerName = GetNameOfContainer();
            var file = new FileInfo(containerName);
            if (!file.Exists)
            {
                file.Create().Close();
                file.Encrypt();
            }
            DeleteKeyFromContainer(keyName, containerName);
            using var writer = file.Exists ? file.AppendText() : file.CreateText();
            writer.WriteLine(keyName + " " + key);
        }

        public static string ReadKeyFromContainer(string keyName)
        {
            var file = new FileInfo(GetNameOfContainer());
            if (file.Exists)
            {
                using var reader = file.OpenText();
                while (!reader.EndOfStream)
                {
                    var lineItems = reader.ReadLine();

                    if (lineItems.IndexOf(keyName) == 0)
                    {
                        return lineItems.Split().Last();
                    }
                }
                throw new Exception("Ключ с таким именем не найден");
            }
            else
            {
                throw new Exception("Файл с ключами не найден");
            }

            throw new Exception("Файл был поврежден или ключ не был найден в файле");
        }

        public static void DeleteKeyFromContainer(string keyName, string containerName)
        {
            File.WriteAllLines(containerName, File.ReadAllLines(containerName).Where(v => v.Trim().IndexOf(keyName) == -1).ToArray());
        }
    }
}
