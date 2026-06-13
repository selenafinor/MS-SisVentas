using Microsoft.EntityFrameworkCore.Migrations;

namespace MSVenta.Inventario.Migrations
{
    public partial class AddFotoToArticulo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "articulo",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "articulo");
        }
    }
}
