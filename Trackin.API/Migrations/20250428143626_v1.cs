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
                name: "Motos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Placa = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    Modelo = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Ano = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    RFIDTag = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DataAquisicao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UltimaManutencao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ImagemReferencia = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    CaracteristicasVisuais = table.Column<string>(type: "CLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motos", x => x.Id);
                });

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
                name: "EventoMoto",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MotoId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Tipo = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UsuarioId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    Observacao = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    FonteEvento = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoMoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventoMoto_Motos_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Camera",
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
                    table.PrimaryKey("PK_Camera", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Camera_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Camera_Patios_PatioId1",
                        column: x => x.PatioId1,
                        principalTable: "Patios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Filiais",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    PatioId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Telefone = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Filiais_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ZonaPatio",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PatioId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    CoordenadaInicialX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CoordenadaInicialY = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CoordenadaFinalX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CoordenadaFinalY = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Cor = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonaPatio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZonaPatio_Patios_PatioId",
                        column: x => x.PatioId,
                        principalTable: "Patios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeteccaoVisual",
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
                    table.PrimaryKey("PK_DeteccaoVisual", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeteccaoVisual_Camera_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Camera",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeteccaoVisual_Motos_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Motos",
                        principalColumn: "Id");
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
                    FilialId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Filiais_FilialId",
                        column: x => x.FilialId,
                        principalTable: "Filiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Camera_PatioId",
                table: "Camera",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_Camera_PatioId1",
                table: "Camera",
                column: "PatioId1");

            migrationBuilder.CreateIndex(
                name: "IX_DeteccaoVisual_CameraId",
                table: "DeteccaoVisual",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_DeteccaoVisual_MotoId",
                table: "DeteccaoVisual",
                column: "MotoId");

            migrationBuilder.CreateIndex(
                name: "IX_EventoMoto_MotoId",
                table: "EventoMoto",
                column: "MotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Filiais_PatioId",
                table: "Filiais",
                column: "PatioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localizacoes_MotoId",
                table: "Localizacoes",
                column: "MotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Localizacoes_PatioId",
                table: "Localizacoes",
                column: "PatioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FilialId",
                table: "Usuarios",
                column: "FilialId");

            migrationBuilder.CreateIndex(
                name: "IX_ZonaPatio_PatioId",
                table: "ZonaPatio",
                column: "PatioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeteccaoVisual");

            migrationBuilder.DropTable(
                name: "EventoMoto");

            migrationBuilder.DropTable(
                name: "Localizacoes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "ZonaPatio");

            migrationBuilder.DropTable(
                name: "Camera");

            migrationBuilder.DropTable(
                name: "Motos");

            migrationBuilder.DropTable(
                name: "Filiais");

            migrationBuilder.DropTable(
                name: "Patios");
        }
    }
}
