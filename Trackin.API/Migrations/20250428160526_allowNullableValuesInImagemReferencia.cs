using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackin.API.Migrations
{
    /// <inheritdoc />
    public partial class allowNullableValuesInImagemReferencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImagemReferencia",
                table: "Motos",
                type: "NVARCHAR2(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "CaracteristicasVisuais",
                table: "Motos",
                type: "CLOB",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "CLOB");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImagemReferencia",
                table: "Motos",
                type: "NVARCHAR2(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CaracteristicasVisuais",
                table: "Motos",
                type: "CLOB",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "CLOB",
                oldNullable: true);
        }
    }
}
