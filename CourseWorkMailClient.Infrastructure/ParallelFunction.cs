using System.Linq;
using CourseWorkMailClient.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows.Controls;
using System.Windows;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using MimeKit;

namespace CourseWorkMailClient.Infrastructure
{
    public static class ParallelFunction
    {
        public static void LoadLastLetter(Folder folder, int count = 10)
        {
            folder.Source.Open(FolderAccess.ReadWrite);

            var uids = folder.Source.Search(SearchQuery.All).Take(count).ToList();
            var lastLetters = folder.Source.Fetch(uids, MessageSummaryItems.Headers);
            var letters = new List<Letter>();
            
            folder.Source.Close();
/*            foreach (var item in lastLetters)
            {
                var newLetter = HandlerService.mapper.Map<MimeMessage, Letter>(new MimeMessage(item.Headers));
                newLetter.Folder = folder;
                letters.Add(newLetter);
            }*/


        }
    }
}
