using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MSVenta.Compras.Migrations
{
    public partial class AddOrdenCompra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orden_compra",
                columns: table => new
                {
                    id_orden = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: true),
                    glosa = table.Column<string>(type: "text", nullable: true),
                    total = table.Column<decimal>(type: "numeric", nullable: false),
                    id_proveedor = table.Column<int>(type: "integer", nullable: true),
                    id_usuario = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orden_compra", x => x.id_orden);
                    table.ForeignKey(
                        name: "FK_orden_compra_proveedor_id_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "proveedor",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "detalle_orden_compra",
                columns: table => new
                {
                    id_detalle_orden = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cantidad = table.Column<decimal>(type: "numeric", nullable: false),
                    precio_uni = table.Column<decimal>(type: "numeric", nullable: false),
                    sub_total = table.Column<decimal>(type: "numeric", nullable: false),
                    nombre_producto = table.Column<string>(type: "text", nullable: true),
                    id_orden = table.Column<int>(type: "integer", nullable: false),
                    id_producto = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalle_orden_compra", x => x.id_detalle_orden);
                    table.ForeignKey(
                        name: "FK_detalle_orden_compra_orden_compra_id_orden",
                        column: x => x.id_orden,
                        principalTable: "orden_compra",
                        principalColumn: "id_orden",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orden_compra_id_proveedor",
                table: "orden_compra",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_orden_compra_id_orden",
                table: "detalle_orden_compra",
                column: "id_orden");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detalle_orden_compra");

            migrationBuilder.DropTable(
                name: "orden_compra");
        }
    }
}