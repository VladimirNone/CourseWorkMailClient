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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=KeysContainer;Trusted_Connection=True;");
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

            modelBuilder.Entity<Letter>()
                .HasMany(l => l.Folders)
                .WithMany(s => s.Letters)
                .UsingEntity(j => j.ToTable("LettersFolders"));

            modelBuilder.Entity<MailServer>().HasData(
                new MailServer { Id = 1, ServerName = "gmail.com" },
                new MailServer { Id = 2, ServerName = "yandex.ru" });

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Login = "CourseWork41@gmail.com", Password = "C9v-EzB-3sT-kfT", MailServerId = 1 });
        }
    }
}
