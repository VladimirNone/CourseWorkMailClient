using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkMailClient.Data.Migrations
{
    public partial class AddRefInUserToInterlocator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "CountOfMessage",
                table: "Folders");

            migrationBuilder.AddColumn<int>(
                name: "InterlocutorId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_InterlocutorId",
                table: "Users",
                column: "InterlocutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Interlocutors_InterlocutorId",
                table: "Users",
                column: "InterlocutorId",
                principalTable: "Interlocutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Interlocutors_InterlocutorId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_InterlocutorId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InterlocutorId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Letters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountOfMessage",
                table: "Folders",
                type: "int",
                nullable: true);
        }
    }
}
