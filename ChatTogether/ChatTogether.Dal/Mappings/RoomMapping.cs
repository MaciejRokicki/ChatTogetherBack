using ChatTogether.Dal.Dbos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatTogether.Dal.Mappings
{
    public class RoomMapping : IEntityTypeConfiguration<RoomDbo>
    {
        public void Configure(EntityTypeBuilder<RoomDbo> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .Property(x => x.MaxPeople)
                .IsRequired();

            builder
                .ToTable("Rooms");
        }
    }
}
