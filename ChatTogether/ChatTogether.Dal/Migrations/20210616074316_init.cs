using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatTogether.Dal.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExampleTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Txt = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Field2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleTable", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ExampleTable",
                columns: new[] { "Id", "Field2", "Txt" },
                values: new object[,]
                {
                    { 1, null, "txt1" },
                    { 2, "field2", "txt2" },
                    { 3, null, "txt3" },
                    { 4, "field4", "txt4" },
                    { 5, null, "txt5" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExampleTable");
        }
    }
}
