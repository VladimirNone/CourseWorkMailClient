using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWorkMailClient.Data.Migrations
{
    public partial class ChangedNamesFakedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterlocutorLetter_Interlocutors_SendersId",
                table: "InterlocutorLetter");

            migrationBuilder.DropForeignKey(
                name: "FK_InterlocutorLetter_Letters_SendedLettersId",
                table: "InterlocutorLetter");

            migrationBuilder.DropForeignKey(
                name: "FK_InterlocutorLetter1_Interlocutors_ReceiversId",
                table: "InterlocutorLetter1");

            migrationBuilder.DropForeignKey(
                name: "FK_InterlocutorLetter1_Letters_ReceivedLettersId",
                table: "InterlocutorLetter1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterlocutorLetter1",
                table: "InterlocutorLetter1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterlocutorLetter",
                table: "InterlocutorLetter");

            migrationBuilder.RenameTable(
                name: "InterlocutorLetter1",
                newName: "ReceiversReceivedLetters");

            migrationBuilder.RenameTable(
                name: "InterlocutorLetter",
                newName: "SendersSendedLetters");

            migrationBuilder.RenameIndex(
                name: "IX_InterlocutorLetter1_ReceiversId",
                table: "ReceiversReceivedLetters",
                newName: "IX_ReceiversReceivedLetters_ReceiversId");

            migrationBuilder.RenameIndex(
                name: "IX_InterlocutorLetter_SendersId",
                table: "SendersSendedLetters",
                newName: "IX_SendersSendedLetters_SendersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReceiversReceivedLetters",
                table: "ReceiversReceivedLetters",
                columns: new[] { "ReceivedLettersId", "ReceiversId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SendersSendedLetters",
                table: "SendersSendedLetters",
                columns: new[] { "SendedLettersId", "SendersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiversReceivedLetters_Interlocutors_ReceiversId",
                table: "ReceiversReceivedLetters",
                column: "ReceiversId",
                principalTable: "Interlocutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiversReceivedLetters_Letters_ReceivedLettersId",
                table: "ReceiversReceivedLetters",
                column: "ReceivedLettersId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SendersSendedLetters_Interlocutors_SendersId",
                table: "SendersSendedLetters",
                column: "SendersId",
                principalTable: "Interlocutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SendersSendedLetters_Letters_SendedLettersId",
                table: "SendersSendedLetters",
                column: "SendedLettersId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiversReceivedLetters_Interlocutors_ReceiversId",
                table: "ReceiversReceivedLetters");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiversReceivedLetters_Letters_ReceivedLettersId",
                table: "ReceiversReceivedLetters");

            migrationBuilder.DropForeignKey(
                name: "FK_SendersSendedLetters_Interlocutors_SendersId",
                table: "SendersSendedLetters");

            migrationBuilder.DropForeignKey(
                name: "FK_SendersSendedLetters_Letters_SendedLettersId",
                table: "SendersSendedLetters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SendersSendedLetters",
                table: "SendersSendedLetters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReceiversReceivedLetters",
                table: "ReceiversReceivedLetters");

            migrationBuilder.RenameTable(
                name: "SendersSendedLetters",
                newName: "InterlocutorLetter");

            migrationBuilder.RenameTable(
                name: "ReceiversReceivedLetters",
                newName: "InterlocutorLetter1");

            migrationBuilder.RenameIndex(
                name: "IX_SendersSendedLetters_SendersId",
                table: "InterlocutorLetter",
                newName: "IX_InterlocutorLetter_SendersId");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiversReceivedLetters_ReceiversId",
                table: "InterlocutorLetter1",
                newName: "IX_InterlocutorLetter1_ReceiversId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterlocutorLetter",
                table: "InterlocutorLetter",
                columns: new[] { "SendedLettersId", "SendersId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterlocutorLetter1",
                table: "InterlocutorLetter1",
                columns: new[] { "ReceivedLettersId", "ReceiversId" });

            migrationBuilder.AddForeignKey(
                name: "FK_InterlocutorLetter_Interlocutors_SendersId",
                table: "InterlocutorLetter",
                column: "SendersId",
                principalTable: "Interlocutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterlocutorLetter_Letters_SendedLettersId",
                table: "InterlocutorLetter",
                column: "SendedLettersId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterlocutorLetter1_Interlocutors_ReceiversId",
                table: "InterlocutorLetter1",
                column: "ReceiversId",
                principalTable: "Interlocutors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterlocutorLetter1_Letters_ReceivedLettersId",
                table: "InterlocutorLetter1",
                column: "ReceivedLettersId",
                principalTable: "Letters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
