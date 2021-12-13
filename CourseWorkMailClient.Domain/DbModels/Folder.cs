using MailKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Domain
{
    public class Folder : Entity, INotifyPropertyChanged
    {
        public string Title { get; set; }
        public int MailServerId { get; set; }

        public int? FolderTypeId { get; set; }
        public FolderType FolderType { get; set; }

        private int? countOfMessage;
        [NotMapped]
        public int? CountOfMessage
        {
            get { return countOfMessage; }
            set
            {
                countOfMessage = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        public MailFolder Source { get; set; }
        
        public List<Letter> Letters { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
