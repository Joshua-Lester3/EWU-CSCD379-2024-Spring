using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhym.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSyllableRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_Syllables_SyllableId",
                table: "Words");

            migrationBuilder.DropIndex(
                name: "IX_Words_SyllableId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "SyllableId",
                table: "Words");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Words",
                newName: "WordId");

            migrationBuilder.AddColumn<int>(
                name: "WordId",
                table: "Syllables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Syllables_WordId",
                table: "Syllables",
                column: "WordId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Syllables_Words_WordId",
                table: "Syllables",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "WordId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Syllables_Words_WordId",
                table: "Syllables");

            migrationBuilder.DropIndex(
                name: "IX_Syllables_WordId",
                table: "Syllables");

            migrationBuilder.DropColumn(
                name: "WordId",
                table: "Syllables");

            migrationBuilder.RenameColumn(
                name: "WordId",
                table: "Words",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "SyllableId",
                table: "Words",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Words_SyllableId",
                table: "Words",
                column: "SyllableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_Syllables_SyllableId",
                table: "Words",
                column: "SyllableId",
                principalTable: "Syllables",
                principalColumn: "SyllableId");
        }
    }
}
