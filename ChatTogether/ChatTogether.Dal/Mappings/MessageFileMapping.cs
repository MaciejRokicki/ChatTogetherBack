using ChatTogether.Dal.Dbos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatTogether.Dal.Mappings
{
    public class MessageFileMapping : IEntityTypeConfiguration<MessageFileDbo>
    {
        public void Configure(EntityTypeBuilder<MessageFileDbo> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.Message)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.MessageId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder
                .Property(x => x.FileName)
                .IsRequired();

            builder
                .Property(x => x.Type)
                .IsRequired();

            builder
                .Property(x => x.SourceName)
                .IsRequired();

            builder
                .Property(x => x.ThumbnailName);

            builder
                .HasQueryFilter(x => !x.Message.IsDeleted);

            builder
                .ToTable("MessageFiles");
        }
    }
}
