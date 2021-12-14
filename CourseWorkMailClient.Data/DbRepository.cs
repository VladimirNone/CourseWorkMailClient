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
    public class DbRepository
    {
        private KeyDbContext db { get; set; }

        public DbRepository(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KeyDbContext>();

            var options = optionsBuilder
                    .UseSqlServer(connectionString)
                    .Options;

            db = new KeyDbContext(options);
        }

        public Interlocutor GetInterlocutor(string email, bool includeKeys = false)
        {
            var interlocators = db.Interlocutors.AsQueryable();
            if (includeKeys)
            {
                interlocators = interlocators.Include(h => h.LastDESRsaKey).Include(h => h.LastMD5RsaKey).Include(h => h.UserLastDESRsaKey).Include(h => h.UserLastMD5RsaKey);
            }
            return interlocators.FirstOrDefault(h => h.Email.ToLower() == email.ToLower());
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

        public void RemoveMessages(List<int> uids, Folder folder)
        {
            db.Letters.RemoveRange(db.Letters.Where(h => uids.Contains(h.UniqueId) && h.FolderId == folder.Id));
        }

        public void SelectAndAddNewFolders(List<Folder> folderFromServer, MailServer mailServer)
        {
            var foldersInDb = db.Folders.Where(h => h.MailServerId == mailServer.Id).Select(h => h.Title);
            var copyFoldersFromServer = folderFromServer.ToList();
            copyFoldersFromServer.RemoveAll(h => foldersInDb.Contains(h.Title));
            db.Folders.AddRange(copyFoldersFromServer);
        }

        /// <summary>
        /// Возвращает сообщения из бд, если таковы есть и добавляет в бд новые и возвращает их в том числе
        /// </summary>
        /// <param name="lettersFromServer"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public List<Letter> SelectAndAddNewLetters(List<Letter> lettersFromServer, Folder folder)
        {
            var lettFromDbUids = db.Letters.Where(h => h.FolderId == folder.Id).Select(h => h.UniqueId).ToList();

            var lettersInDb = lettersFromServer.Where(h => lettFromDbUids.Contains(h.UniqueId)).ToList();
            var lettNotInDb = lettersFromServer.Where(h => !lettFromDbUids.Contains(h.UniqueId)).ToList();

            for (int i = 0; i < lettersInDb.Count; i++)
            {
                var mes = GetMessage(lettersInDb[i].UniqueId, folder);
                mes.MD5RsaKey = lettersInDb[i].MD5RsaKey;
                mes.DESRsaKey = lettersInDb[i].DESRsaKey;
            }

            foreach (var item in lettersFromServer)
            {
                item.FolderId = folder.Id;
            }

            db.Letters.AddRange(lettNotInDb);

            SaveChanged();

            return lettersInDb.Concat(lettNotInDb).ToList();
        }

        public Interlocutor GetOrCreateInterlocutor(string interlocutorEmail)
        {
            var old = GetInterlocutor(interlocutorEmail);
            if(old == null)
            {
                db.Interlocutors.Add(new Interlocutor() {  Email = interlocutorEmail });
                SaveChanged();
            }
            return GetInterlocutor(interlocutorEmail);
        }

        public List<int> GetUniqueIds(Folder folder)
        {
            return db.Letters.Where(h => h.FolderId == folder.Id).OrderBy(h => h.UniqueId).Select(h => h.UniqueId).ToList();
        }

        public Folder GetFolder(MailServer mailServer, string folderName)
        {
            return db.Folders.FirstOrDefault(h => h.MailServerId == mailServer.Id && h.Title == folderName);
        }

        public Folder GetFolder(MailServer mailServer, int folderTypeId)
        {
            return db.Folders.FirstOrDefault(h => h.MailServerId == mailServer.Id && h.FolderTypeId == folderTypeId);
        }

        public List<Folder> GetFolders(MailServer mailServer)
        {
            return db.Folders.Where(h => h.MailServerId == mailServer.Id).ToList();
        }

        public int GetFolderTypeId(string folderName)
        {
            var folder = db.FolderTypes.FirstOrDefault(h => h.TypeName == folderName);
            return folder == null ? -1 : folder.Id;
        }

        public User GetUser(string login)
        {
            return db.Users.Include(h=>h.MailServer).FirstOrDefault(h => h.Login == login);
        }

        public Letter GetMessage(int uniqueId, Folder folder, bool lightVersion = true)
        {
            var query = db.Letters.AsQueryable();

            if (!lightVersion)
            {
                query = query.Include(h => h.MD5RsaKey).Include(h => h.DESRsaKey).Include(h => h.Senders).Include(h => h.Receivers).Include(h => h.Attachments);
            }

            return query.FirstOrDefault(h => h.UniqueId == uniqueId && h.FolderId == folder.Id);
        }

        public List<Letter> GetMessages(int folderId, List<int> uids)
        {
            return db.Letters.Where(h => h.FolderId == folderId && uids.Contains(h.UniqueId)).ToList();
        }

        public int GetCountOfMessages(int folderId)
        {
            return db.Folders.Include(h => h.Letters).First(h => h.Id == folderId).Letters.Count;
        }

        public void SaveChanged()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
