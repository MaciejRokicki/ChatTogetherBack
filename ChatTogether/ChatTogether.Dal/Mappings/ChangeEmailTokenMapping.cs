using ChatTogether.Dal.Dbos.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatTogether.Dal.Mappings
{
    public class ChangeEmailTokenMapping : IEntityTypeConfiguration<ChangeEmailTokenDbo>
    {
        public void Configure(EntityTypeBuilder<ChangeEmailTokenDbo> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.Account)
                .WithOne(x => x.ChangeEmailTokenDbo)
                .HasForeignKey<ChangeEmailTokenDbo>(x => x.AccountId);

            builder
                .Property(x => x.Token)
                .IsRequired();

            builder
                .ToTable("ChangeEmailTokens");
        }
    }
}
