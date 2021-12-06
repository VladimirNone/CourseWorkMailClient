using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkMailClient.Data.Migrations
{
    public partial class AddAbilitityStoreLetterInSeveralFolders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "LettersFolders",
                columns: table => new
                {
                    FoldersId = table.Column<int>(type: "int", nullable: false),
                    LettersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LettersFolders", x => new { x.FoldersId, x.LettersId });
                    table.ForeignKey(
                        name: "FK_LettersFolders_Folders_FoldersId",
                        column: x => x.FoldersId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LettersFolders_Letters_LettersId",
                        column: x => x.LettersId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LettersFolders_LettersId",
                table: "LettersFolders",
                column: "LettersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LettersFolders");

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
    }
}
