using CourseWorkMailClient.Domain;
using CourseWorkMailClient.Domain.Keys;
using Microsoft.EntityFrameworkCore;
using System;

namespace CourseWorkMailClient.Data
{
    public class KeyDbContext : DbContext
    {
        public DbSet<DESRsaKey> DESRsaKeys { get; set; }
        public DbSet<MD5RsaKey> MD5RsaKeys { get; set; }
        public DbSet<Interlocutor> Interlocutors { get; set; }
        public DbSet<Letter> Letters { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<MailServer> MailServers { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FolderType> FolderTypes { get; set; }

        public KeyDbContext(DbContextOptions<KeyDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Letter>()
                .HasMany(l => l.Senders)
                .WithMany(s => s.SendedLetters)
                .UsingEntity(j => j.ToTable("SendersSendedLetters"));

            modelBuilder.Entity<Letter>()
                .HasMany(l => l.Receivers)
                .WithMany(s => s.ReceivedLetters)
                .UsingEntity(j => j.ToTable("ReceiversReceivedLetters"));

            modelBuilder.Entity<MailServer>().HasData(
                new MailServer { Id = 1, ServerName = "gmail.com" },
                new MailServer { Id = 2, ServerName = "yandex.ru" });
        }
    }
}
