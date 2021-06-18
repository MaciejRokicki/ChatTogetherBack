using ChatTogether.Dal.Dbos.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatTogether.Dal.Mappings
{
    public class ConfirmEmailTokenMapping : IEntityTypeConfiguration<ConfirmEmailTokenDbo>
    {
        public void Configure(EntityTypeBuilder<ConfirmEmailTokenDbo> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.Account)
                .WithOne(x => x.ConfirmEmailTokenDbo)
                .HasForeignKey<ConfirmEmailTokenDbo>(x => x.AccountId);

            builder
                .Property(x => x.Token)
                .IsRequired();

            builder
                .ToTable("ConfirmEmailTokens");
        }
    }
}
