using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraductorBasico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Frases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Español = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ingles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pronunciacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImagenesDiccionario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutaImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagenesDiccionario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FraseImagenes",
                columns: table => new
                {
                    FraseId = table.Column<int>(type: "int", nullable: false),
                    ImagenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FraseImagenes", x => new { x.FraseId, x.ImagenId });
                    table.ForeignKey(
                        name: "FK_FraseImagenes_Frases_FraseId",
                        column: x => x.FraseId,
                        principalTable: "Frases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FraseImagenes_ImagenesDiccionario_ImagenId",
                        column: x => x.ImagenId,
                        principalTable: "ImagenesDiccionario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FraseImagenes_ImagenId",
                table: "FraseImagenes",
                column: "ImagenId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagenesDiccionario_Categoria",
                table: "ImagenesDiccionario",
                column: "Categoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FraseImagenes");

            migrationBuilder.DropTable(
                name: "Frases");

            migrationBuilder.DropTable(
                name: "ImagenesDiccionario");
        }
    }
}
