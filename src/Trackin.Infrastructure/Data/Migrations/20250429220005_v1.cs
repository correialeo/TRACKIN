using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackin.API.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Endereco = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Cidade = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Pais = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DimensaoX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    DimensaoY = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PlantaBaixa = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PatioId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Posicao = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    PosicaoX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PosicaoY = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Altura = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    AnguloVisao = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    URL = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    PatioId1 = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cameras_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cameras_Patios_PatioId1",
                        column: x => x.PatioId1,
                        principalTable: "Patios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Motos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PatioId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Placa = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    Modelo = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Ano = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    RFIDTag = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    UltimaManutencao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ImagemReferencia = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    CaracteristicasVisuais = table.Column<string>(type: "CLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Motos_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    PatioId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZonasPatio",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PatioId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    TipoZona = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CoordenadaInicialX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CoordenadaInicialY = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CoordenadaFinalX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CoordenadaFinalY = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Cor = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonasPatio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZonasPatio_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeteccoesVisuais",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MotoId = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CameraId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CoordenadaXImagem = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CoordenadaYImagem = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CoordenadaXPatio = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CoordenadaYPatio = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Confianca = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    ImagemCaptura = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeteccoesVisuais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeteccoesVisuais_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeteccoesVisuais_Motos_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Motos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Localizacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MotoId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PatioId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CoordenadaX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CoordenadaY = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    FonteDados = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    Confiabilidade = table.Column<double>(type: "BINARY_DOUBLE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localizacoes_Motos_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Localizacoes_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MotoId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Tipo = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UsuarioId = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SensorId = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CameraId = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    Observacao = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    FonteEvento = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventos_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Eventos_Motos_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Eventos_SensorRFID_SensorId",
                        column: x => x.SensorId,
                        principalTable: "SensorRFID",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Eventos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_PatioId",
                table: "Cameras",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_PatioId1",
                table: "Cameras",
                column: "PatioId1");

            migrationBuilder.CreateIndex(
                name: "IX_DeteccoesVisuais_CameraId",
                table: "DeteccoesVisuais",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_DeteccoesVisuais_MotoId",
                table: "DeteccoesVisuais",
                column: "MotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_CameraId",
                table: "Eventos",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_MotoId",
                table: "Eventos",
                column: "MotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_SensorId",
                table: "Eventos",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioId",
                table: "Eventos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Localizacoes_MotoId",
                table: "Localizacoes",
                column: "MotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Localizacoes_PatioId",
                table: "Localizacoes",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_Motos_PatioId",
                table: "Motos",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorRFID_PatioId",
                table: "SensorRFID",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorRFID_ZonaPatioId",
                table: "SensorRFID",
                column: "ZonaPatioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PatioId",
                table: "Usuarios",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_ZonasPatio_PatioId",
                table: "ZonasPatio",
                column: "PatioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeteccoesVisuais");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Localizacoes");

            migrationBuilder.DropTable(
                name: "Cameras");

            migrationBuilder.DropTable(
                name: "SensorRFID");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Motos");

            migrationBuilder.DropTable(
                name: "ZonasPatio");

            migrationBuilder.DropTable(
                name: "Patios");
        }
    }
}
