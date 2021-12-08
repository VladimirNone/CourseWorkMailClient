using CourseWorkMailClient.Domain;
using CourseWorkMailClient.Domain.Keys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public Interlocutor GetInterlocutor(string email)
        {
            return db.Interlocutors.FirstOrDefault(h => h.Email == email);
        }

        public void AddMessage(Letter letter)
        {
            db.Letters.Add(letter);
        }

        public void AddMessages(List<Letter> letters)
        {
            db.Letters.AddRange(letters);
        }

        public void AddFolder(Folder folder)
        {
            db.Folders.Add(folder);
        }

        public void AddUser(User user, string mail)
        {
            user.MailServer = db.MailServers.FirstOrDefault(h => h.ServerName.Contains(mail));
            db.Users.Add(user);
        }

        public void RemoveMessage(int letterId)
        {
            db.Letters.Remove(db.Letters.Find(letterId));
        }

        public void SelectAndAddNewFolders(List<Folder> folderFromServer, MailServer mailServer)
        {
            var foldersInDb = db.Folders.Where(h => h.MailServerId == mailServer.Id).Select(h => h.Title);
            var copyFoldersFromServer = folderFromServer.ToList();
            copyFoldersFromServer.RemoveAll(h => foldersInDb.Contains(h.Title));
            db.Folders.AddRange(copyFoldersFromServer);
        }

        public void SelectAndAddNewLetters(List<Letter> lettersFromServer, Folder folder)
        {
            var lettFromServerMessageIds = lettersFromServer.Select(h => h.MessageId);

            var lettersInDb = db.Letters.Where(h => lettFromServerMessageIds.Contains(h.MessageId)).Include(h => h.Folders).ToList();
            var lettNotInDb = lettersFromServer.Except(lettersInDb);

            foreach (var item in lettersFromServer)
            {
                if (item.Folders == null)
                    item.Folders = new List<Folder>();

                item.Folders.Add(folder);
            }

            db.Letters.AddRange(lettNotInDb);
        }

        public Folder GetFolder(MailServer mailServer, string folderName)
        {
            return db.Folders.FirstOrDefault(h => h.MailServerId == mailServer.Id && h.Title == folderName);
        }

        public List<Folder> GetFolders(MailServer mailServer)
        {
            return db.Folders.Where(h => h.MailServerId == mailServer.Id).ToList();
        }

        public List<User> GetUsers()
        {
            return db.Users.Include(h => h.MailServer).Include(h => h.Interlocutor).ToList();
        }

        public Letter GetMessage(string messageId, bool lightVersion = true)
        {
            var query = db.Letters.AsQueryable();

            if (!lightVersion)
            {
                query = query.Include(h => h.MD5RsaKey).Include(h => h.DESRsaKey).Include(h => h.Senders).Include(h => h.Receivers).Include(h => h.Attachments);
            }

            return query.FirstOrDefault(h => h.MessageId == messageId);
        }

        public List<Letter> GetMessages(int folderId)
        {
            return db.Folders.Include(h => h.Letters).First(h => h.Id == folderId).Letters;
        }

        public void SaveChanged()
        {
            db.SaveChanges();
        }
    }
}
