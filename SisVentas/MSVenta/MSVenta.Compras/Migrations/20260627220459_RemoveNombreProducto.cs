using Microsoft.EntityFrameworkCore.Migrations;

namespace MSVenta.Compras.Migrations
{
    public partial class RemoveNombreProducto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nombre_producto",
                table: "catalogo_proveedor",
                type: "text",
                nullable: true);
        }
    }
}
