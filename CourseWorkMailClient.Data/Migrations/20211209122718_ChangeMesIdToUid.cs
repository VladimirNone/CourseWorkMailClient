using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkMailClient.Data.Migrations
{
    public partial class ChangeMesIdToUid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Letters");

            migrationBuilder.AddColumn<int>(
                name: "UniqueId",
                table: "Letters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "Letters");

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "Letters",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
