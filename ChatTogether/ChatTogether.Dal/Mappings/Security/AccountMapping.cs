using ChatTogether.Dal.Dbos.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

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
                .Property(x => x.CreationDate)
                .HasDefaultValue(DateTime.Now)
                .IsRequired();

            builder
                .Property(x => x.IsConfirmed)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .ToTable("Accounts");
        }
    }
}
