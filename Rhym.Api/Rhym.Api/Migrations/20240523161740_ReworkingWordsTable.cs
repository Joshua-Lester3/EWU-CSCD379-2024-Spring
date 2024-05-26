using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhym.Api.Migrations
{
    /// <inheritdoc />
    public partial class ReworkingWordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pronunciation",
                table: "Words",
                newName: "Syllables");

            migrationBuilder.AddColumn<string>(
                name: "Phonemes",
                table: "Words",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phonemes",
                table: "Words");

            migrationBuilder.RenameColumn(
                name: "Syllables",
                table: "Words",
                newName: "Pronunciation");
        }
    }
}
