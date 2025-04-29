using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackin.API.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Camera_Patios_PatioId",
                table: "Camera");

            migrationBuilder.DropForeignKey(
                name: "FK_Camera_Patios_PatioId1",
                table: "Camera");

            migrationBuilder.DropForeignKey(
                name: "FK_DeteccaoVisual_Camera_CameraId",
                table: "DeteccaoVisual");

            migrationBuilder.DropForeignKey(
                name: "FK_DeteccaoVisual_Motos_MotoId",
                table: "DeteccaoVisual");

            migrationBuilder.DropForeignKey(
                name: "FK_EventoMoto_Motos_MotoId",
                table: "EventoMoto");

            migrationBuilder.DropForeignKey(
                name: "FK_ZonaPatio_Patios_PatioId",
                table: "ZonaPatio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ZonaPatio",
                table: "ZonaPatio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventoMoto",
                table: "EventoMoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeteccaoVisual",
                table: "DeteccaoVisual");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Camera",
                table: "Camera");

            migrationBuilder.RenameTable(
                name: "ZonaPatio",
                newName: "ZonasPatio");

            migrationBuilder.RenameTable(
                name: "EventoMoto",
                newName: "Eventos");

            migrationBuilder.RenameTable(
                name: "DeteccaoVisual",
                newName: "DeteccoesVisuais");

            migrationBuilder.RenameTable(
                name: "Camera",
                newName: "Cameras");

            migrationBuilder.RenameIndex(
                name: "IX_ZonaPatio_PatioId",
                table: "ZonasPatio",
                newName: "IX_ZonasPatio_PatioId");

            migrationBuilder.RenameIndex(
                name: "IX_EventoMoto_MotoId",
                table: "Eventos",
                newName: "IX_Eventos_MotoId");

            migrationBuilder.RenameIndex(
                name: "IX_DeteccaoVisual_MotoId",
                table: "DeteccoesVisuais",
                newName: "IX_DeteccoesVisuais_MotoId");

            migrationBuilder.RenameIndex(
                name: "IX_DeteccaoVisual_CameraId",
                table: "DeteccoesVisuais",
                newName: "IX_DeteccoesVisuais_CameraId");

            migrationBuilder.RenameIndex(
                name: "IX_Camera_PatioId1",
                table: "Cameras",
                newName: "IX_Cameras_PatioId1");

            migrationBuilder.RenameIndex(
                name: "IX_Camera_PatioId",
                table: "Cameras",
                newName: "IX_Cameras_PatioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ZonasPatio",
                table: "ZonasPatio",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Eventos",
                table: "Eventos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeteccoesVisuais",
                table: "DeteccoesVisuais",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cameras",
                table: "Cameras",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cameras_Patios_PatioId",
                table: "Cameras",
                column: "PatioId",
                principalTable: "Patios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cameras_Patios_PatioId1",
                table: "Cameras",
                column: "PatioId1",
                principalTable: "Patios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeteccoesVisuais_Cameras_CameraId",
                table: "DeteccoesVisuais",
                column: "CameraId",
                principalTable: "Cameras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeteccoesVisuais_Motos_MotoId",
                table: "DeteccoesVisuais",
                column: "MotoId",
                principalTable: "Motos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Motos_MotoId",
                table: "Eventos",
                column: "MotoId",
                principalTable: "Motos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZonasPatio_Patios_PatioId",
                table: "ZonasPatio",
                column: "PatioId",
                principalTable: "Patios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cameras_Patios_PatioId",
                table: "Cameras");

            migrationBuilder.DropForeignKey(
                name: "FK_Cameras_Patios_PatioId1",
                table: "Cameras");

            migrationBuilder.DropForeignKey(
                name: "FK_DeteccoesVisuais_Cameras_CameraId",
                table: "DeteccoesVisuais");

            migrationBuilder.DropForeignKey(
                name: "FK_DeteccoesVisuais_Motos_MotoId",
                table: "DeteccoesVisuais");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Motos_MotoId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_ZonasPatio_Patios_PatioId",
                table: "ZonasPatio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ZonasPatio",
                table: "ZonasPatio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Eventos",
                table: "Eventos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeteccoesVisuais",
                table: "DeteccoesVisuais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cameras",
                table: "Cameras");

            migrationBuilder.RenameTable(
                name: "ZonasPatio",
                newName: "ZonaPatio");

            migrationBuilder.RenameTable(
                name: "Eventos",
                newName: "EventoMoto");

            migrationBuilder.RenameTable(
                name: "DeteccoesVisuais",
                newName: "DeteccaoVisual");

            migrationBuilder.RenameTable(
                name: "Cameras",
                newName: "Camera");

            migrationBuilder.RenameIndex(
                name: "IX_ZonasPatio_PatioId",
                table: "ZonaPatio",
                newName: "IX_ZonaPatio_PatioId");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_MotoId",
                table: "EventoMoto",
                newName: "IX_EventoMoto_MotoId");

            migrationBuilder.RenameIndex(
                name: "IX_DeteccoesVisuais_MotoId",
                table: "DeteccaoVisual",
                newName: "IX_DeteccaoVisual_MotoId");

            migrationBuilder.RenameIndex(
                name: "IX_DeteccoesVisuais_CameraId",
                table: "DeteccaoVisual",
                newName: "IX_DeteccaoVisual_CameraId");

            migrationBuilder.RenameIndex(
                name: "IX_Cameras_PatioId1",
                table: "Camera",
                newName: "IX_Camera_PatioId1");

            migrationBuilder.RenameIndex(
                name: "IX_Cameras_PatioId",
                table: "Camera",
                newName: "IX_Camera_PatioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ZonaPatio",
                table: "ZonaPatio",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventoMoto",
                table: "EventoMoto",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeteccaoVisual",
                table: "DeteccaoVisual",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Camera",
                table: "Camera",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Camera_Patios_PatioId",
                table: "Camera",
                column: "PatioId",
                principalTable: "Patios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Camera_Patios_PatioId1",
                table: "Camera",
                column: "PatioId1",
                principalTable: "Patios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeteccaoVisual_Camera_CameraId",
                table: "DeteccaoVisual",
                column: "CameraId",
                principalTable: "Camera",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeteccaoVisual_Motos_MotoId",
                table: "DeteccaoVisual",
                column: "MotoId",
                principalTable: "Motos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventoMoto_Motos_MotoId",
                table: "EventoMoto",
                column: "MotoId",
                principalTable: "Motos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZonaPatio_Patios_PatioId",
                table: "ZonaPatio",
                column: "PatioId",
                principalTable: "Patios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
