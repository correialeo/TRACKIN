using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackin.API.Migrations
{
    /// <inheritdoc />
    public partial class refacEventoMoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UsuarioId",
                table: "Eventos",
                type: "NUMBER(19)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CameraId",
                table: "Eventos",
                type: "NUMBER(19)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SensorId",
                table: "Eventos",
                type: "NUMBER(19)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_CameraId",
                table: "Eventos",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_SensorId",
                table: "Eventos",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioId",
                table: "Eventos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Cameras_CameraId",
                table: "Eventos",
                column: "CameraId",
                principalTable: "Cameras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_SensorRFID_SensorId",
                table: "Eventos",
                column: "SensorId",
                principalTable: "SensorRFID",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Usuarios_UsuarioId",
                table: "Eventos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Cameras_CameraId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_SensorRFID_SensorId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Usuarios_UsuarioId",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_CameraId",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_SensorId",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_UsuarioId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "CameraId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "SensorId",
                table: "Eventos");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Eventos",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "NUMBER(19)",
                oldNullable: true);
        }
    }
}
