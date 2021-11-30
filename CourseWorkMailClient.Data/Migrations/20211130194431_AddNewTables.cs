using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkMailClient.Data.Migrations
{
    public partial class AddNewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interlocutors_DESRsaKeys_DESRsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropForeignKey(
                name: "FK_Interlocutors_MD5RsaKeys_MD5RsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropIndex(
                name: "IX_Interlocutors_DESRsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropIndex(
                name: "IX_Interlocutors_MD5RsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropColumn(
                name: "DESRsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropColumn(
                name: "MD5RsaKeyId",
                table: "Interlocutors");

            migrationBuilder.AddColumn<int>(
                name: "LastDESRsaKeyId",
                table: "Interlocutors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastMD5RsaKeyId",
                table: "Interlocutors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailServerId = table.Column<int>(type: "int", nullable: false),
                    CountOfMessage = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Letters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalMessage = table.Column<bool>(type: "bit", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PathToFullMessageFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MD5RsaKeyId = table.Column<int>(type: "int", nullable: true),
                    DESRsaKeyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Letters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Letters_DESRsaKeys_DESRsaKeyId",
                        column: x => x.DESRsaKeyId,
                        principalTable: "DESRsaKeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Letters_MD5RsaKeys_MD5RsaKeyId",
                        column: x => x.MD5RsaKeyId,
                        principalTable: "MD5RsaKeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MailServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LetterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_Letters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InterlocutorLetter",
                columns: table => new
                {
                    SendedLettersId = table.Column<int>(type: "int", nullable: false),
                    SendersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterlocutorLetter", x => new { x.SendedLettersId, x.SendersId });
                    table.ForeignKey(
                        name: "FK_InterlocutorLetter_Interlocutors_SendersId",
                        column: x => x.SendersId,
                        principalTable: "Interlocutors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterlocutorLetter_Letters_SendedLettersId",
                        column: x => x.SendedLettersId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterlocutorLetter1",
                columns: table => new
                {
                    ReceivedLettersId = table.Column<int>(type: "int", nullable: false),
                    ReceiversId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterlocutorLetter1", x => new { x.ReceivedLettersId, x.ReceiversId });
                    table.ForeignKey(
                        name: "FK_InterlocutorLetter1_Interlocutors_ReceiversId",
                        column: x => x.ReceiversId,
                        principalTable: "Interlocutors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterlocutorLetter1_Letters_ReceivedLettersId",
                        column: x => x.ReceivedLettersId,
                        principalTable: "Letters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MailServers",
                columns: new[] { "Id", "ServerName" },
                values: new object[] { 1, "gmail.com" });

            migrationBuilder.InsertData(
                table: "MailServers",
                columns: new[] { "Id", "ServerName" },
                values: new object[] { 2, "yandex.ru" });

            migrationBuilder.CreateIndex(
                name: "IX_Interlocutors_LastDESRsaKeyId",
                table: "Interlocutors",
                column: "LastDESRsaKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Interlocutors_LastMD5RsaKeyId",
                table: "Interlocutors",
                column: "LastMD5RsaKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_LetterId",
                table: "Attachments",
                column: "LetterId");

            migrationBuilder.CreateIndex(
                name: "IX_InterlocutorLetter_SendersId",
                table: "InterlocutorLetter",
                column: "SendersId");

            migrationBuilder.CreateIndex(
                name: "IX_InterlocutorLetter1_ReceiversId",
                table: "InterlocutorLetter1",
                column: "ReceiversId");

            migrationBuilder.CreateIndex(
                name: "IX_Letters_DESRsaKeyId",
                table: "Letters",
                column: "DESRsaKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Letters_MD5RsaKeyId",
                table: "Letters",
                column: "MD5RsaKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interlocutors_DESRsaKeys_LastDESRsaKeyId",
                table: "Interlocutors",
                column: "LastDESRsaKeyId",
                principalTable: "DESRsaKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Interlocutors_MD5RsaKeys_LastMD5RsaKeyId",
                table: "Interlocutors",
                column: "LastMD5RsaKeyId",
                principalTable: "MD5RsaKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interlocutors_DESRsaKeys_LastDESRsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropForeignKey(
                name: "FK_Interlocutors_MD5RsaKeys_LastMD5RsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "InterlocutorLetter");

            migrationBuilder.DropTable(
                name: "InterlocutorLetter1");

            migrationBuilder.DropTable(
                name: "MailServers");

            migrationBuilder.DropTable(
                name: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Interlocutors_LastDESRsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropIndex(
                name: "IX_Interlocutors_LastMD5RsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropColumn(
                name: "LastDESRsaKeyId",
                table: "Interlocutors");

            migrationBuilder.DropColumn(
                name: "LastMD5RsaKeyId",
                table: "Interlocutors");

            migrationBuilder.AddColumn<int>(
                name: "DESRsaKeyId",
                table: "Interlocutors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MD5RsaKeyId",
                table: "Interlocutors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Interlocutors_DESRsaKeyId",
                table: "Interlocutors",
                column: "DESRsaKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Interlocutors_MD5RsaKeyId",
                table: "Interlocutors",
                column: "MD5RsaKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interlocutors_DESRsaKeys_DESRsaKeyId",
                table: "Interlocutors",
                column: "DESRsaKeyId",
                principalTable: "DESRsaKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interlocutors_MD5RsaKeys_MD5RsaKeyId",
                table: "Interlocutors",
                column: "MD5RsaKeyId",
                principalTable: "MD5RsaKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
