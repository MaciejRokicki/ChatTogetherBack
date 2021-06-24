using ChatTogether.Dal.Dbos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatTogether.Dal.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<UserDbo>
    {
        public void Configure(EntityTypeBuilder<UserDbo> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Nickname)
                .HasMaxLength(100);

            builder
                .Property(x => x.FirstName)
                .HasMaxLength(100);

            builder
                .Property(x => x.LastName)
                .HasMaxLength(100);

            builder
                .Property(x => x.BirthDate);

            builder
                .Property(x => x.City)
                .HasMaxLength(100);

            builder
                .Property(x => x.Description)
                .HasMaxLength(600);

            builder
                .HasOne(x => x.Account)
                .WithOne(x => x.User)
                .HasForeignKey<UserDbo>(x => x.AccountId);

            builder
                .ToTable("Users");
        }
    }
}
