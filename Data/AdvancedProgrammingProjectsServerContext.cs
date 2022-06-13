#nullable disable
using Microsoft.EntityFrameworkCore;
using Domain.DatabaseEntryModels;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class AdvancedProgrammingProjectsServerContext : DbContext

    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public AdvancedProgrammingProjectsServerContext(IConfiguration config) {
            this._configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            connectionString = _configuration["ConnectionStrings:MariaDB"];
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<RegisteredUser>().HasKey(e => e.username);
            modelBuilder.Entity<RegisteredUser>().Property(e => e.username).HasMaxLength(127);
            modelBuilder.Entity<PendingUser>().HasKey(e => e.username);
            modelBuilder.Entity<PendingUser>().Property(e => e.username).HasMaxLength(127);
            modelBuilder.Entity<Message>().HasKey(e => e.id);
            modelBuilder.Entity<Conversation>().HasKey(e => e.Id);
            modelBuilder.Entity<SecretQuestion>().HasKey(e => e.Id);
            modelBuilder.Entity<Contact>().HasKey(e => new {e.contactOf, e.id});
            modelBuilder.Entity<Contact>().Property(e => e.id).HasMaxLength(127);
            modelBuilder.Entity<Contact>().Property(e => e.contactOf).HasMaxLength(127);
            modelBuilder.Entity<Rating>().HasKey(e => e.Id);
            modelBuilder.Entity<RefreshToken>().HasKey(e => e.Id);
            modelBuilder.Entity<RefreshToken>().HasOne<RegisteredUser>();
        }

        public DbSet<Contact> Contact { get; set; }

        public DbSet<Conversation> Conversation { get; set; }

        public DbSet<Message> Message { get; set; }

        public DbSet<RegisteredUser> RegisteredUser { get; set; }

        public DbSet<SecretQuestion> SecretQuestion { get; set; }

        public DbSet<PendingUser> PendingUser { get; set; }

        public DbSet<Rating> Rating { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }
    }
}
