using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkMailClient.Data.Migrations
{
    public partial class ChangedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "LocalMessage",
                table: "Letters",
                newName: "Seen");

            migrationBuilder.AddColumn<int>(
                name: "UserLastDESRsaKeyId",
                table: "Interlocutors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserLastMD5RsaKeyId",
                table: "Interlocutors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FolderTypeId",
                table: "Folders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FolderTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interlocutors_UserLastDESRsaKeyId",
                table: "Interlocutors",
                column: "UserLastDESRsaKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Interlocutors_UserLastMD5RsaKeyId",
                table: "Interlocutors",
                column: "UserLastMD5RsaKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_FolderTypeId",
                table: "Folders",
                column: "FolderTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_FolderTypes_FolderTypeId",
                table: "Folders",
                column: "FolderTypeId",
                principalTable: "FolderTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interlocutors_DESRsaKeys_UserLastDESRsaKeyId",
                table: "Interlocutors",
                column: "UserLastDESRsaKeyId",
                principalTable: "DESRsaKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interlocutors_MD5RsaKeys_UserLastMD5RsaKeyId",
                table: "Interlocutors",
                column: "UserLastMD5RsaKeyId",
                principalTable: "MD5RsaKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_FolderTypes_FolderTypeId",
                table: "Folders");

            migrationBuilder.DropForeignKey(
                name: "FK_Interlocutors_DESRsaKeys_UserLastDESRsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropForeignKey(
                name: "FK_Interlocutors_MD5RsaKeys_UserLastMD5RsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropTable(
                name: "FolderTypes");

            migrationBuilder.DropIndex(
                name: "IX_Interlocutors_UserLastDESRsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropIndex(
                name: "IX_Interlocutors_UserLastMD5RsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropIndex(
                name: "IX_Folders_FolderTypeId",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "UserLastDESRsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropColumn(
                name: "UserLastMD5RsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropColumn(
                name: "FolderTypeId",
                table: "Folders");

            migrationBuilder.RenameColumn(
                name: "Seen",
                table: "Letters",
                newName: "LocalMessage");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "InterlocutorId", "Login", "MailServerId", "Password" },
                values: new object[] { 1, null, "CourseWork41@gmail.com", 1, "C9v-EzB-3sT-kfT" });
        }
    }
}
