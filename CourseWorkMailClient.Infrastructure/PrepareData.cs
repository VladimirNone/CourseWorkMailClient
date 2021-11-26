using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Infrastructure
{
    public static class PrepareData
    {
        public static string GetParsedName(string folderName)
        {
            return folderName == "INBOX" ? "Входящие" : folderName;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
