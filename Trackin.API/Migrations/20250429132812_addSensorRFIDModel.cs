using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackin.API.Migrations
{
    /// <inheritdoc />
    public partial class addSensorRFIDModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SensorRFID",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ZonaPatioId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PatioId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Posicao = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PosicaoX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PosicaoY = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Altura = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    AnguloVisao = table.Column<double>(type: "BINARY_DOUBLE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorRFID", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorRFID_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SensorRFID_ZonasPatio_ZonaPatioId",
                        column: x => x.ZonaPatioId,
                        principalTable: "ZonasPatio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorRFID_PatioId",
                table: "SensorRFID",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorRFID_ZonaPatioId",
                table: "SensorRFID",
                column: "ZonaPatioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorRFID");
        }
    }
}
