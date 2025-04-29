using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackin.API.Migrations
{
    /// <inheritdoc />
    public partial class addTipoZona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoZona",
                table: "ZonasPatio",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoZona",
                table: "ZonasPatio");
        }
    }
}
