using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkMailClient.Data.Migrations
{
    public partial class AddLinkToFolderInLetter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "Letters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Letters_FolderId",
                table: "Letters",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Folders_FolderId",
                table: "Letters",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Folders_FolderId",
                table: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Letters_FolderId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Letters");
        }
    }
}
