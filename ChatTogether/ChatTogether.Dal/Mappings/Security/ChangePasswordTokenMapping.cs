using ChatTogether.Dal.Dbos.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatTogether.Dal.Mappings.Security
{
    public class ChangePasswordTokenMapping : IEntityTypeConfiguration<ChangePasswordTokenDbo>
    {
        public void Configure(EntityTypeBuilder<ChangePasswordTokenDbo> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.Account)
                .WithOne(x => x.ChangePasswordTokenDbo)
                .HasForeignKey<ChangePasswordTokenDbo>(x => x.AccountId);

            builder
                .Property(x => x.Token)
                .IsRequired();

            builder
                .ToTable("ChangePasswordTokens");
        }
    }
}
