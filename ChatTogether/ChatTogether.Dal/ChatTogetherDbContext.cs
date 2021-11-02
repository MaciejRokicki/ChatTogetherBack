using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Mappings;
using ChatTogether.Dal.Mappings.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatTogether.Dal
{
    public class ChatTogetherDbContext : DbContext
    {
        public ChatTogetherDbContext(DbContextOptions<ChatTogetherDbContext> options) : base(options) { }

        public DbSet<AccountDbo> AccountDbos { get; set; }
        public DbSet<BlockedAccountDbo> BlockedAccountDbos { get; set; }
        public DbSet<ConfirmEmailTokenDbo> confirmEmailTokenDbos { get; set; }
        public DbSet<ChangeEmailTokenDbo> ChangeEmailTokenDbos { get; set; }
        public DbSet<ChangePasswordTokenDbo> ChangePasswordTokenDbos { get; set; }

        public DbSet<UserDbo> UserDbos { get; set; }
        public DbSet<RoomDbo> RoomDbos { get; set; }
        public DbSet<MessageDbo> MessageDbos { get; set; }
        public DbSet<MessageFileDbo> MessageFileDbos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountMapping());
            modelBuilder.ApplyConfiguration(new BlockedAccountMapping());
            modelBuilder.ApplyConfiguration(new ConfirmEmailTokenMapping());
            modelBuilder.ApplyConfiguration(new ChangeEmailTokenMapping());
            modelBuilder.ApplyConfiguration(new ChangePasswordTokenMapping());

            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new RoomMapping());
            modelBuilder.ApplyConfiguration(new MessageMapping());
            modelBuilder.ApplyConfiguration(new MessageFileMapping());
        }

        public override int SaveChanges()
        {
            SoftDeleteSaveChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SoftDeleteSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void SoftDeleteSaveChanges()
        {
            foreach(EntityEntry entry in ChangeTracker.Entries())
            {
                bool isSoftDelete = entry.CurrentValues.Properties.Any(x => x.Name == "IsDeleted");

                if (entry.State == EntityState.Deleted && isSoftDelete)
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                }
            }
        }
    }
}
