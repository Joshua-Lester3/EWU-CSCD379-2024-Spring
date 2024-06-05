using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhym.Api.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rhymes",
                columns: table => new
                {
                    RhymeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phonemes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SyllablesPronunciation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlainTextSyllables = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rhymes", x => x.RhymeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rhymes");
        }
    }
}
