using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace films_website.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieFormViewModelId",
                table: "Geners",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MovieFormViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    StoryLine = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Poster = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    GenreId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieFormViewModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Geners_MovieFormViewModelId",
                table: "Geners",
                column: "MovieFormViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Geners_MovieFormViewModel_MovieFormViewModelId",
                table: "Geners",
                column: "MovieFormViewModelId",
                principalTable: "MovieFormViewModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Geners_MovieFormViewModel_MovieFormViewModelId",
                table: "Geners");

            migrationBuilder.DropTable(
                name: "MovieFormViewModel");

            migrationBuilder.DropIndex(
                name: "IX_Geners_MovieFormViewModelId",
                table: "Geners");

            migrationBuilder.DropColumn(
                name: "MovieFormViewModelId",
                table: "Geners");
        }
    }
}
