using ChatTogether.Dal.Dbos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatTogether.Dal.Mappings
{
    public class MessageMapping : IEntityTypeConfiguration<MessageDbo>
    {
        public void Configure(EntityTypeBuilder<MessageDbo> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Message)
                .IsRequired();

            builder
                .Property(x => x.SendTime)
                .IsRequired();

            builder
                .Property(x => x.ReceivedTime)
                .IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Room)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.RoomId);

            builder
                .ToTable("Messages");
        }
    }
}
