using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhym.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSyllable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Syllables",
                table: "Words",
                newName: "SyllablesPronunciation");

            migrationBuilder.AddColumn<int>(
                name: "SyllableId",
                table: "Words",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Syllables",
                columns: table => new
                {
                    SyllableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Syllables = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Syllables", x => x.SyllableId);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_Syllables_SyllableId",
                table: "Words");

            migrationBuilder.DropTable(
                name: "Syllables");

            migrationBuilder.DropIndex(
                name: "IX_Words_SyllableId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "SyllableId",
                table: "Words");

            migrationBuilder.RenameColumn(
                name: "SyllablesPronunciation",
                table: "Words",
                newName: "Syllables");
        }
    }
}
