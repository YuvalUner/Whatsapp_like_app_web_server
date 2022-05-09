#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AdvancedProgrammingProjectsServer.Models;

namespace AdvancedProgrammingProjectsServer.Data
{
    public class AdvancedProgrammingProjectsServerContext : DbContext
    {

        private const string connectionString = "server=localhost;port=3306;database=Items;user=root;password=dangit65";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<RegisteredUser>().HasKey(e => e.username);
            modelBuilder.Entity<PendingUser>().HasKey(e => e.username);
            modelBuilder.Entity<Message>().HasKey(e => e.key);
            modelBuilder.Entity<Conversation>().HasKey(e => e.Id);
            modelBuilder.Entity<SecretQuestion>().HasKey(e => e.Id);
            modelBuilder.Entity<Contact>().HasKey(e => new {e.contactOf, e.name});
        }

        public DbSet<AdvancedProgrammingProjectsServer.Models.Contact> Contact { get; set; }

        public DbSet<AdvancedProgrammingProjectsServer.Models.Conversation> Conversation { get; set; }

        public DbSet<AdvancedProgrammingProjectsServer.Models.Message> Message { get; set; }

        public DbSet<AdvancedProgrammingProjectsServer.Models.RegisteredUser> RegisteredUser { get; set; }

        public DbSet<AdvancedProgrammingProjectsServer.Models.SecretQuestion> SecretQuestion { get; set; }

        public DbSet<AdvancedProgrammingProjectsServer.Models.PendingUser> PendingUser { get; set; }
    }
}
