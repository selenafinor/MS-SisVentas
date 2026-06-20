using Microsoft.EntityFrameworkCore;
using MSVenta.Venta.Models;

namespace MSVenta.Venta.Repositories
{
    public class ContextDatabase : DbContext
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Models.Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── CLIENTE ──────────────────────────────────────────────
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("cliente");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasColumnName("id_cliente");
                entity.Property(c => c.Nombre).HasColumnName("nombre");
                entity.Property(c => c.Paterno).HasColumnName("paterno");
                entity.Property(c => c.Materno).HasColumnName("materno");
                entity.Property(c => c.Telefono).HasColumnName("telefono");
                entity.Property(c => c.Correo).HasColumnName("correo");
                entity.Property(c => c.Nit).HasColumnName("nit");
                entity.Property(c => c.Direccion).HasColumnName("direccion");
                entity.Property(c => c.Estado).HasColumnName("estado");
            });

            // ── NOTA VENTA ────────────────────────────────────────────
            modelBuilder.Entity<Models.Venta>(entity =>
            {
                entity.ToTable("nota_venta");
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Id).HasColumnName("id_venta");
                entity.Property(v => v.Fecha).HasColumnName("fecha_venta");
                entity.Property(v => v.Hora).HasColumnName("hora_venta");
                entity.Property(v => v.MontoTotal).HasColumnName("monto_total");
                entity.Property(v => v.Glosa).HasColumnName("glosa");
                entity.Property(v => v.Estado).HasColumnName("estado");
                entity.Property(v => v.TipoPago).HasColumnName("tipo_pago");
                entity.Property(v => v.PagoConfirmado).HasColumnName("pago_confirmado");
                entity.Property(v => v.IdTransaccionQr).HasColumnName("id_transaccion_qr");
                entity.Property(v => v.ClienteId).HasColumnName("id_cliente");
                entity.Property(v => v.UsuarioId).HasColumnName("id_usuario");
                entity.HasOne(v => v.Cliente)
                      .WithMany()
                      .HasForeignKey(v => v.ClienteId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(v => v.Detalles)
                      .WithOne(d => d.Venta)
                      .HasForeignKey(d => d.VentaId);
            });

            // ── DETALLE VENTA ─────────────────────────────────────────
            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.ToTable("detalle_venta");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Id).HasColumnName("id_detalle_venta");
                entity.Property(d => d.VentaId).HasColumnName("id_venta");
                entity.Property(d => d.Id_Producto).HasColumnName("id_producto");
                entity.Property(d => d.NombreProducto).HasColumnName("nombre_producto");
                entity.Property(d => d.Id_Almacen).HasColumnName("id_almacen");
                entity.Property(d => d.NombreAlmacen).HasColumnName("nombre_almacen");
                entity.Property(d => d.Cantidad).HasColumnName("cantidad");
                entity.Property(d => d.PrecioUni).HasColumnName("precio_uni");
                entity.Property(d => d.PrecioSubtotal).HasColumnName("precio_subtotal");
                entity.HasOne(d => d.Venta)
                      .WithMany(v => v.Detalles)
                      .HasForeignKey(d => d.VentaId);
            });
        }
    }
}