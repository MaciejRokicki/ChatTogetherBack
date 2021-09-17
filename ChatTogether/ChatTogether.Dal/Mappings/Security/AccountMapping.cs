using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatTogether.Dal.Mappings.Security
{
    public class AccountMapping : IEntityTypeConfiguration<AccountDbo>
    {
        public void Configure(EntityTypeBuilder<AccountDbo> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Password)
                .IsRequired();

            builder
                .Property(x => x.Created)
                .IsRequired();

            builder
                .Property(x => x.IsConfirmed)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(x => x.Role)
                .HasConversion<string>()
                .HasDefaultValue(Role.USER)
                .IsRequired();

            builder
                .HasOne(x => x.BlockedAccountDbo)
                .WithOne(x => x.Account)
                .HasForeignKey<AccountDbo>(x => x.BlockedAccountId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .ToTable("Accounts");
        }
    }
}
