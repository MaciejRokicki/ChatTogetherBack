using ChatTogether.Dal.Dbos.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatTogether.Dal.Mappings.Security
{
    public class BlockedAccountMapping : IEntityTypeConfiguration<BlockedAccountDbo>
    {
        public void Configure(EntityTypeBuilder<BlockedAccountDbo> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Reason)
                .IsRequired();

            builder
                .Property(x => x.BlockedTo);

            builder
                .Property(x => x.Created)
                .IsRequired();

            builder
                .ToTable("BlockedAccounts");
        }
    }
}
