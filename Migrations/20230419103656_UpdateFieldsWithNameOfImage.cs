using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonWebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldsWithNameOfImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameOfImage",
                table: "Pokemons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameOfImage",
                table: "Pokemons");
        }
    }
}
