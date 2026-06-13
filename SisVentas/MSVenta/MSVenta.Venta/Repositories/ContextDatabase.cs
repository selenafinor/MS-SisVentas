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
            modelBuilder.Entity<Cliente>().ToTable("cliente");
            modelBuilder.Entity<Models.Venta>().ToTable("nota_venta");
            modelBuilder.Entity<DetalleVenta>().ToTable("detalle_venta");

            modelBuilder.Entity<Models.Venta>(entity =>
            {
                entity.Property(v => v.ClienteId).HasColumnName("id_cliente");
                entity.Property(v => v.UsuarioId).HasColumnName("id_usuario");
                entity.Property(v => v.MontoTotal).HasColumnName("monto_total");
                entity.Property(v => v.TipoPago).HasColumnName("tipo_pago");

                entity.HasOne(v => v.Cliente)
                      .WithMany()
                      .HasForeignKey(v => v.ClienteId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.ToTable("detalle_venta");

                entity.Property(dv => dv.VentaId).HasColumnName("id_venta");
                entity.Property(dv => dv.Id_Producto).HasColumnName("id_producto");
                entity.Property(dv => dv.Id_Almacen).HasColumnName("id_almacen");
                entity.Property(dv => dv.PrecioUni).HasColumnName("precio_uni");
                entity.Property(dv => dv.PrecioSubtotal).HasColumnName("precio_subtotal");

                entity.HasOne(dv => dv.Venta)
                      .WithMany()
                      .HasForeignKey(dv => dv.VentaId);
            });
        }
    }
}