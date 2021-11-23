using CourseWorkMailClient.Domain;
using S22.Imap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Infrastructure
{
    public static class Handlers
    {
        private static List<CustomMessage> actualMessages;
        public static List<CustomMessage> ActualMessages
        {
            get => actualMessages;
            set
            {
                actualMessages = value;
                if(ActualMessagesChanged != null)
                    ActualMessagesChanged(actualMessages);
            }
        }

        public static Action<List<CustomMessage>> ActualMessagesChanged { get; set; }

        public static void Auth(string login, string password)
        {
            //CourseWork41@gmail.com
            //C9v-EzB-3sT-kfT

            ImapHandler = new ImapHandler(login, password);
            KitImapHandler = new KitImapHandler(login, password);
        }

        public static ImapHandler ImapHandler { get; set; }
        public static KitImapHandler KitImapHandler { get; set; }
    }
}
