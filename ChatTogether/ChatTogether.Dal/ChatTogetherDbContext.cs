using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ChatTogether.Dal
{
    public class ChatTogetherDbContext : DbContext
    {
        public ChatTogetherDbContext(DbContextOptions<ChatTogetherDbContext> options) : base(options) { }

        public DbSet<AccountDbo> AccountDbos { get; set; }
        public DbSet<ConfirmEmailTokenDbo> confirmEmailTokenDbos { get; set; }
        public DbSet<ChangeEmailTokenDbo> ChangeEmailTokenDbos { get; set; }
        public DbSet<ChangePasswordTokenDbo> ChangePasswordTokenDbos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountMapping());
            modelBuilder.ApplyConfiguration(new ConfirmEmailTokenMapping());
            modelBuilder.ApplyConfiguration(new ChangeEmailTokenMapping());
            modelBuilder.ApplyConfiguration(new ChangePasswordTokenMapping());
        }
    }
}
