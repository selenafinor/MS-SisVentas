using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MSVenta.Compras.Migrations
{
    public partial class AddAdquisicion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adquisicion",
                columns: table => new
                {
                    id_adquisicion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: true),
                    glosa = table.Column<string>(type: "text", nullable: true),
                    id_orden = table.Column<int>(type: "integer", nullable: true),
                    id_proveedor = table.Column<int>(type: "integer", nullable: true),
                    id_usuario = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adquisicion", x => x.id_adquisicion);
                    table.ForeignKey(
                        name: "FK_adquisicion_orden_compra_id_orden",
                        column: x => x.id_orden,
                        principalTable: "orden_compra",
                        principalColumn: "id_orden",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_adquisicion_proveedor_id_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "proveedor",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "detalle_adquisicion",
                columns: table => new
                {
                    id_detalle_adq = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cantidad = table.Column<decimal>(type: "numeric", nullable: false),
                    precio_uni = table.Column<decimal>(type: "numeric", nullable: false),
                    sub_total = table.Column<decimal>(type: "numeric", nullable: false),
                    nombre_producto = table.Column<string>(type: "text", nullable: true),
                    id_adquisicion = table.Column<int>(type: "integer", nullable: false),
                    id_producto = table.Column<int>(type: "integer", nullable: true),
                    id_almacen = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalle_adquisicion", x => x.id_detalle_adq);
                    table.ForeignKey(
                        name: "FK_detalle_adquisicion_adquisicion_id_adquisicion",
                        column: x => x.id_adquisicion,
                        principalTable: "adquisicion",
                        principalColumn: "id_adquisicion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_adquisicion_id_orden",
                table: "adquisicion",
                column: "id_orden");

            migrationBuilder.CreateIndex(
                name: "IX_adquisicion_id_proveedor",
                table: "adquisicion",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_adquisicion_id_adquisicion",
                table: "detalle_adquisicion",
                column: "id_adquisicion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detalle_adquisicion");

            migrationBuilder.DropTable(
                name: "adquisicion");
        }
    }
}
