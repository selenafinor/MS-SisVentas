using System;
using Microsoft.EntityFrameworkCore.Migrations;
namespace MSVenta.Inventario.Migrations
{
    public partial class AddGesPrecio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ges_precio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Id_Articulo = table.Column<int>(type: "int", nullable: false),
                    PrecioCompra = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    MetodoInventario = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ges_precio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ges_precio_articulo_Id_Articulo",
                        column: x => x.Id_Articulo,
                        principalTable: "articulo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "IX_ges_precio_Id_Articulo",
                table: "ges_precio",
                column: "Id_Articulo");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ges_precio");
        }
    }
}