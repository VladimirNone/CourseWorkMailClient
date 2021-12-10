﻿using CourseWorkMailClient.Domain;
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

            foreach (var item in lettersFromServer)
            {
                item.FolderId = folder.Id;
            }

            db.Letters.AddRange(lettNotInDb);

            return lettersInDb.Concat(lettNotInDb).ToList();
        }

        public List<int> GetUniqueIds(Folder folder)
        {
            return db.Letters.Where(h => h.FolderId == folder.Id).OrderBy(h=>h.UniqueId).Select(h => h.UniqueId).ToList();
        }

        public Folder GetFolderWithLetters(MailServer mailServer, string folderName)
        {
            return db.Folders.FirstOrDefault(h => h.MailServerId == mailServer.Id && h.Title == folderName);
        }

        public Folder GetFolder(MailServer mailServer, string folderName)
        {
            return db.Folders.FirstOrDefault(h => h.MailServerId == mailServer.Id && h.Title == folderName);
        }

        public List<Folder> GetFolders(MailServer mailServer)
        {
            return db.Folders.Where(h => h.MailServerId == mailServer.Id).ToList();
        }

        public User GetUser(string login)
        {
            return db.Users.FirstOrDefault(h => h.Login == login);
        }

        public List<User> GetUsers()
        {
            return db.Users.Include(h => h.MailServer).Include(h => h.Interlocutor).ToList();
        }

        public Letter GetMessage(int uniqueId, bool lightVersion = true)
        {
            var query = db.Letters.AsQueryable();

            if (!lightVersion)
            {
                query = query.Include(h => h.MD5RsaKey).Include(h => h.DESRsaKey).Include(h => h.Senders).Include(h => h.Receivers).Include(h => h.Attachments);
            }

            return query.FirstOrDefault(h => h.UniqueId == uniqueId);
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
    }
}
