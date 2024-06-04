using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhym.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWordConnectionToSyllable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Word",
                table: "Syllables",
                newName: "WordKey");

            migrationBuilder.RenameColumn(
                name: "Syllables",
                table: "Syllables",
                newName: "PlainTextSyllables");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WordKey",
                table: "Syllables",
                newName: "Word");

            migrationBuilder.RenameColumn(
                name: "PlainTextSyllables",
                table: "Syllables",
                newName: "Syllables");
        }
    }
}
