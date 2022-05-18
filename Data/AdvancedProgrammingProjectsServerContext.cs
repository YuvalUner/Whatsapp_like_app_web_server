﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.DatabaseEntryModels;

namespace Data
{
    public class AdvancedProgrammingProjectsServerContext : DbContext
    {

        private const string connectionString = "***REMOVED***";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<RegisteredUser>().HasKey(e => e.username);
            modelBuilder.Entity<PendingUser>().HasKey(e => e.username);
            modelBuilder.Entity<Message>().HasKey(e => e.id);
            modelBuilder.Entity<Conversation>().HasKey(e => e.Id);
            modelBuilder.Entity<SecretQuestion>().HasKey(e => e.Id);
            modelBuilder.Entity<Contact>().HasKey(e => new {e.contactOf, e.id});
            modelBuilder.Entity<Rating>().HasKey(e => e.Id);
            modelBuilder.Entity<RefreshToken>().HasKey(e => e.Id);
            modelBuilder.Entity<RefreshToken>().HasOne<RegisteredUser>();
            modelBuilder.Entity<ActiveConnection>().HasKey(e => new { e.ConnectionId, e.UserAgent, e.Username });
        }

        public DbSet<Contact> Contact { get; set; }

        public DbSet<Conversation> Conversation { get; set; }

        public DbSet<Message> Message { get; set; }

        public DbSet<RegisteredUser> RegisteredUser { get; set; }

        public DbSet<SecretQuestion> SecretQuestion { get; set; }

        public DbSet<PendingUser> PendingUser { get; set; }

        public DbSet<Rating> Rating { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }

        public DbSet<ActiveConnection> ActiveConnection { get; set; }
    }
}
