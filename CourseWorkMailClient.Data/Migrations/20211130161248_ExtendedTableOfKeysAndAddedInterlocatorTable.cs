using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkMailClient.Data.Migrations
{
    public partial class ExtendedTableOfKeysAndAddedInterlocatorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeathTime",
                table: "MD5RsaKeys",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LifeTime",
                table: "MD5RsaKeys",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeathTime",
                table: "DESRsaKeys",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LifeTime",
                table: "DESRsaKeys",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Interlocutors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MD5RsaKeyId = table.Column<int>(type: "int", nullable: false),
                    DESRsaKeyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interlocutors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interlocutors_DESRsaKeys_DESRsaKeyId",
                        column: x => x.DESRsaKeyId,
                        principalTable: "DESRsaKeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interlocutors_MD5RsaKeys_MD5RsaKeyId",
                        column: x => x.MD5RsaKeyId,
                        principalTable: "MD5RsaKeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interlocutors_DESRsaKeyId",
                table: "Interlocutors",
                column: "DESRsaKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Interlocutors_MD5RsaKeyId",
                table: "Interlocutors",
                column: "MD5RsaKeyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interlocutors");

            migrationBuilder.DropColumn(
                name: "DeathTime",
                table: "MD5RsaKeys");

            migrationBuilder.DropColumn(
                name: "LifeTime",
                table: "MD5RsaKeys");

            migrationBuilder.DropColumn(
                name: "DeathTime",
                table: "DESRsaKeys");

            migrationBuilder.DropColumn(
                name: "LifeTime",
                table: "DESRsaKeys");
        }
    }
}
