using ChatTogether.Dal.Dbos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatTogether.Dal.Mappings
{
    public class ExampleMapping : IEntityTypeConfiguration<ExampleDbo>
    {

        private readonly ExampleDbo[] examples = new ExampleDbo[]
        {
            new ExampleDbo() { Id = 1, Txt = "txt1" },
            new ExampleDbo() { Id = 2, Txt = "txt2", Field2 = "field2" },
            new ExampleDbo() { Id = 3, Txt = "txt3" },
            new ExampleDbo() { Id = 4, Txt = "txt4", Field2 = "field4" },
            new ExampleDbo() { Id = 5, Txt = "txt5" },
        };

        public void Configure(EntityTypeBuilder<ExampleDbo> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Txt)
                .IsRequired()
                .HasMaxLength(45);

            builder
                .Property(x => x.Field2);

            builder
                .HasData(examples);

            builder
                .ToTable("ExampleTable");
        }
    }
}
