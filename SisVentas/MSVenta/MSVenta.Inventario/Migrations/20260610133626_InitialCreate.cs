using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSVenta.Inventario.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "almacen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Direccion = table.Column<string>(type: "text", nullable: true),
                    CantidadMax = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_almacen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "marca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marca", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "nota_egreso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Glosa = table.Column<string>(type: "text", nullable: true),
                    Motivo = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nota_egreso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "nota_ingreso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Glosa = table.Column<string>(type: "text", nullable: true),
                    Motivo = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nota_ingreso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "unidad_medida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Abreviatura = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unidad_medida", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "traspaso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Glosa = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario = table.Column<int>(type: "int", nullable: false),
                    Id_AlmacenOrigen = table.Column<int>(type: "int", nullable: false),
                    Id_AlmacenDestino = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_traspaso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_traspaso_almacen_Id_AlmacenDestino",
                        column: x => x.Id_AlmacenDestino,
                        principalTable: "almacen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_traspaso_almacen_Id_AlmacenOrigen",
                        column: x => x.Id_AlmacenOrigen,
                        principalTable: "almacen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "articulo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: true),
                    Id_Marca = table.Column<int>(type: "int", nullable: false),
                    Id_Categoria = table.Column<int>(type: "int", nullable: false),
                    Id_UnidadMedida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articulo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_articulo_categoria_Id_Categoria",
                        column: x => x.Id_Categoria,
                        principalTable: "categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_articulo_marca_Id_Marca",
                        column: x => x.Id_Marca,
                        principalTable: "marca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_articulo_unidad_medida_Id_UnidadMedida",
                        column: x => x.Id_UnidadMedida,
                        principalTable: "unidad_medida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "articulo_almacen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    StockMin = table.Column<int>(type: "int", nullable: false),
                    StockMax = table.Column<int>(type: "int", nullable: false),
                    Id_Articulo = table.Column<int>(type: "int", nullable: false),
                    Id_Almacen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articulo_almacen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_articulo_almacen_almacen_Id_Almacen",
                        column: x => x.Id_Almacen,
                        principalTable: "almacen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_articulo_almacen_articulo_Id_Articulo",
                        column: x => x.Id_Articulo,
                        principalTable: "articulo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalle_egreso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "text", nullable: true),
                    Id_Egreso = table.Column<int>(type: "int", nullable: false),
                    Id_ArticuloAlmacen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalle_egreso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detalle_egreso_articulo_almacen_Id_ArticuloAlmacen",
                        column: x => x.Id_ArticuloAlmacen,
                        principalTable: "articulo_almacen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalle_egreso_nota_egreso_Id_Egreso",
                        column: x => x.Id_Egreso,
                        principalTable: "nota_egreso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalle_ingreso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioCompra = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Observacion = table.Column<string>(type: "text", nullable: true),
                    Id_Ingreso = table.Column<int>(type: "int", nullable: false),
                    Id_ArticuloAlmacen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalle_ingreso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detalle_ingreso_articulo_almacen_Id_ArticuloAlmacen",
                        column: x => x.Id_ArticuloAlmacen,
                        principalTable: "articulo_almacen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalle_ingreso_nota_ingreso_Id_Ingreso",
                        column: x => x.Id_Ingreso,
                        principalTable: "nota_ingreso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalle_traspaso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Id_Traspaso = table.Column<int>(type: "int", nullable: false),
                    Id_ArticuloAlmacen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalle_traspaso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detalle_traspaso_articulo_almacen_Id_ArticuloAlmacen",
                        column: x => x.Id_ArticuloAlmacen,
                        principalTable: "articulo_almacen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalle_traspaso_traspaso_Id_Traspaso",
                        column: x => x.Id_Traspaso,
                        principalTable: "traspaso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_articulo_Id_Categoria",
                table: "articulo",
                column: "Id_Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_articulo_Id_Marca",
                table: "articulo",
                column: "Id_Marca");

            migrationBuilder.CreateIndex(
                name: "IX_articulo_Id_UnidadMedida",
                table: "articulo",
                column: "Id_UnidadMedida");

            migrationBuilder.CreateIndex(
                name: "IX_articulo_almacen_Id_Almacen",
                table: "articulo_almacen",
                column: "Id_Almacen");

            migrationBuilder.CreateIndex(
                name: "IX_articulo_almacen_Id_Articulo",
                table: "articulo_almacen",
                column: "Id_Articulo");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_egreso_Id_ArticuloAlmacen",
                table: "detalle_egreso",
                column: "Id_ArticuloAlmacen");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_egreso_Id_Egreso",
                table: "detalle_egreso",
                column: "Id_Egreso");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_ingreso_Id_ArticuloAlmacen",
                table: "detalle_ingreso",
                column: "Id_ArticuloAlmacen");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_ingreso_Id_Ingreso",
                table: "detalle_ingreso",
                column: "Id_Ingreso");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_traspaso_Id_ArticuloAlmacen",
                table: "detalle_traspaso",
                column: "Id_ArticuloAlmacen");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_traspaso_Id_Traspaso",
                table: "detalle_traspaso",
                column: "Id_Traspaso");

            migrationBuilder.CreateIndex(
                name: "IX_traspaso_Id_AlmacenDestino",
                table: "traspaso",
                column: "Id_AlmacenDestino");

            migrationBuilder.CreateIndex(
                name: "IX_traspaso_Id_AlmacenOrigen",
                table: "traspaso",
                column: "Id_AlmacenOrigen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detalle_egreso");

            migrationBuilder.DropTable(
                name: "detalle_ingreso");

            migrationBuilder.DropTable(
                name: "detalle_traspaso");

            migrationBuilder.DropTable(
                name: "nota_egreso");

            migrationBuilder.DropTable(
                name: "nota_ingreso");

            migrationBuilder.DropTable(
                name: "articulo_almacen");

            migrationBuilder.DropTable(
                name: "traspaso");

            migrationBuilder.DropTable(
                name: "articulo");

            migrationBuilder.DropTable(
                name: "almacen");

            migrationBuilder.DropTable(
                name: "categoria");

            migrationBuilder.DropTable(
                name: "marca");

            migrationBuilder.DropTable(
                name: "unidad_medida");
        }
    }
}