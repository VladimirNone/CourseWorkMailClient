using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkMailClient.Data.Migrations
{
    public partial class ChangedNameOfTableForDESRsaKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DesRsaKeys",
                table: "DesRsaKeys");

            migrationBuilder.RenameTable(
                name: "DesRsaKeys",
                newName: "DESRsaKeys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DESRsaKeys",
                table: "DESRsaKeys",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DESRsaKeys",
                table: "DESRsaKeys");

            migrationBuilder.RenameTable(
                name: "DESRsaKeys",
                newName: "DesRsaKeys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesRsaKeys",
                table: "DesRsaKeys",
                column: "Id");
        }
    }
}
