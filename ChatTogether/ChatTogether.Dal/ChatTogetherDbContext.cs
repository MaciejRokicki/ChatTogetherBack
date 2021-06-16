using Microsoft.EntityFrameworkCore;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Mappings;

namespace ChatTogether.Dal
{
    public class ChatTogetherDbContext : DbContext
    {
        public ChatTogetherDbContext(DbContextOptions<ChatTogetherDbContext> options) : base(options) { }

        public DbSet<ExampleDbo> ExampleDbos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExampleMapping());
        }
    }
}
