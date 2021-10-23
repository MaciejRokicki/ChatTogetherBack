using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatTogether.Dal.Migrations
{
    public partial class MessageFiles4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailPath",
                table: "MessageFiles",
                newName: "ThumbnailName");

            migrationBuilder.RenameColumn(
                name: "SourcePath",
                table: "MessageFiles",
                newName: "SourceName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailName",
                table: "MessageFiles",
                newName: "ThumbnailPath");

            migrationBuilder.RenameColumn(
                name: "SourceName",
                table: "MessageFiles",
                newName: "SourcePath");
        }
    }
}
