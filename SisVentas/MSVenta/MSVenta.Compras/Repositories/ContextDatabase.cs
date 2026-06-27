using Microsoft.EntityFrameworkCore;
using MSVenta.Compras.Models;

namespace MSVenta.Compras.Repositories
{
    public class ContextDatabase : DbContext
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options) { }

        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<NotaCompra> NotasCompra { get; set; }
        public DbSet<DetalleCompra> DetallesCompra { get; set; }
        public DbSet<CatalogoProveedor> CatalogoProveedores { get; set; }
        public DbSet<OrdenCompra> OrdenesCompra { get; set; }
        public DbSet<DetalleOrdenCompra> DetallesOrdenCompra { get; set; }
        public DbSet<Adquisicion> Adquisiciones { get; set; }
        public DbSet<DetalleAdquisicion> DetallesAdquisicion { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.ToTable("proveedor");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_proveedor");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Telefono).HasColumnName("telefono");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Direccion).HasColumnName("direccion");
                entity.Property(e => e.Nit).HasColumnName("nit");
                entity.Property(e => e.Contacto).HasColumnName("contacto");
                entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
                entity.Property(e => e.Estado).HasColumnName("estado");
            });

            modelBuilder.Entity<NotaCompra>(entity =>
            {
                entity.ToTable("nota_compra");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_compra");
                entity.Property(e => e.FechaCompra).HasColumnName("fecha_compra");
                entity.Property(e => e.TotalCompra).HasColumnName("total_compra");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.Glosa).HasColumnName("glosa");
                entity.Property(e => e.TipoPago).HasColumnName("tipo_pago");
                entity.Property(e => e.ProveedorId).HasColumnName("id_proveedor");
                entity.Property(e => e.UsuarioId).HasColumnName("id_usuario");

                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.NotasCompra)
                      .HasForeignKey(e => e.ProveedorId);
            });

            modelBuilder.Entity<DetalleCompra>(entity =>
            {
                entity.ToTable("detalle_compra");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_detalle_compra");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.PrecioUni).HasColumnName("precio_uni");
                entity.Property(e => e.SubTotal).HasColumnName("sub_total");
                entity.Property(e => e.NombreProducto).HasColumnName("nombre_producto");
                entity.Property(e => e.CompraId).HasColumnName("id_compra");
                entity.Property(e => e.ProductoId).HasColumnName("id_producto");

                entity.HasOne(e => e.Compra)
                      .WithMany(c => c.Detalles)
                      .HasForeignKey(e => e.CompraId);
            });

            modelBuilder.Entity<CatalogoProveedor>(entity =>
            {
                entity.ToTable("catalogo_proveedor");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_catalogo");
                entity.Property(e => e.PrecioUnitario).HasColumnName("precio_unitario");
                entity.Property(e => e.StockDisponible).HasColumnName("stock_disponible");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.NombreProducto).HasColumnName("nombre_producto");
                entity.Property(e => e.ProveedorId).HasColumnName("id_proveedor");
                entity.Property(e => e.ProductoId).HasColumnName("id_producto");

                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.Catalogo)
                      .HasForeignKey(e => e.ProveedorId);
            });
            modelBuilder.Entity<OrdenCompra>(entity =>
            {
                entity.ToTable("orden_compra");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_orden");
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.Glosa).HasColumnName("glosa");
                entity.Property(e => e.Total).HasColumnName("total");
                entity.Property(e => e.ProveedorId).HasColumnName("id_proveedor");
                entity.Property(e => e.UsuarioId).HasColumnName("id_usuario");

                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.Ordenes)
                      .HasForeignKey(e => e.ProveedorId);
            });

            modelBuilder.Entity<DetalleOrdenCompra>(entity =>
            {
                entity.ToTable("detalle_orden_compra");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_detalle_orden");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.PrecioUni).HasColumnName("precio_uni");
                entity.Property(e => e.SubTotal).HasColumnName("sub_total");
                entity.Property(e => e.NombreProducto).HasColumnName("nombre_producto");
                entity.Property(e => e.OrdenId).HasColumnName("id_orden");
                entity.Property(e => e.ProductoId).HasColumnName("id_producto");

                entity.HasOne(e => e.Orden)
                      .WithMany(o => o.Detalles)
                      .HasForeignKey(e => e.OrdenId);
            });
            modelBuilder.Entity<Adquisicion>(entity =>
            {
                entity.ToTable("adquisicion");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_adquisicion");
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.Glosa).HasColumnName("glosa");
                entity.Property(e => e.OrdenId).HasColumnName("id_orden");
                entity.Property(e => e.ProveedorId).HasColumnName("id_proveedor");
                entity.Property(e => e.UsuarioId).HasColumnName("id_usuario");

                entity.HasOne(e => e.Orden)
                      .WithMany()
                      .HasForeignKey(e => e.OrdenId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.Adquisiciones)
                      .HasForeignKey(e => e.ProveedorId);
            });

            modelBuilder.Entity<DetalleAdquisicion>(entity =>
            {
                entity.ToTable("detalle_adquisicion");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_detalle_adq");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.PrecioUni).HasColumnName("precio_uni");
                entity.Property(e => e.SubTotal).HasColumnName("sub_total");
                entity.Property(e => e.NombreProducto).HasColumnName("nombre_producto");
                entity.Property(e => e.AdquisicionId).HasColumnName("id_adquisicion");
                entity.Property(e => e.ProductoId).HasColumnName("id_producto");
                entity.Property(e => e.AlmacenId).HasColumnName("id_almacen");

                entity.HasOne(e => e.Adquisicion)
                      .WithMany(a => a.Detalles)
                      .HasForeignKey(e => e.AdquisicionId);
            });
        }
    }
}