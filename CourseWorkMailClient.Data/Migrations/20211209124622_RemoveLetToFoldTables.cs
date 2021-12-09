using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkMailClient.Data.Migrations
{
    public partial class RemoveLetToFoldTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LettersFolders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
