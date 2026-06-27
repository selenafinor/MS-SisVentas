using Microsoft.EntityFrameworkCore;
using MSVenta.Inventario.Models;

namespace MSVenta.Inventario.Repositories
{
    public class ContextDatabase : DbContext
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options) { }

        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<UnidadMedida> UnidadesMedida { get; set; }
        public DbSet<Almacen> Almacenes { get; set; }
        public DbSet<ArticuloAlmacen> ArticulosAlmacenes { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<DetalleIngreso> DetallesIngreso { get; set; }
        public DbSet<Egreso> Egresos { get; set; }
        public DbSet<DetalleEgreso> DetallesEgreso { get; set; }
        public DbSet<Traspaso> Traspasos { get; set; }
        public DbSet<DetalleTraspaso> DetallesTraspaso { get; set; }
        public DbSet<GesPrecio> GesPrecios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Nombres de tablas
            modelBuilder.Entity<Articulo>().ToTable("articulo");
            modelBuilder.Entity<Marca>().ToTable("marca");
            modelBuilder.Entity<Categoria>().ToTable("categoria");
            modelBuilder.Entity<UnidadMedida>().ToTable("unidad_medida");
            modelBuilder.Entity<Almacen>().ToTable("almacen");
            modelBuilder.Entity<ArticuloAlmacen>().ToTable("articulo_almacen");
            modelBuilder.Entity<Ingreso>().ToTable("nota_ingreso");
            modelBuilder.Entity<DetalleIngreso>().ToTable("detalle_ingreso");
            modelBuilder.Entity<Egreso>().ToTable("nota_egreso");
            modelBuilder.Entity<DetalleEgreso>().ToTable("detalle_egreso");
            modelBuilder.Entity<Traspaso>().ToTable("traspaso");
            modelBuilder.Entity<DetalleTraspaso>().ToTable("detalle_traspaso");

            // Relaciones Articulo
            modelBuilder.Entity<Articulo>()
                .HasOne(a => a.Marca)
                .WithMany()
                .HasForeignKey(a => a.Id_Marca);

            modelBuilder.Entity<Articulo>()
                .HasOne(a => a.Categoria)
                .WithMany()
                .HasForeignKey(a => a.Id_Categoria);

            modelBuilder.Entity<Articulo>()
                .HasOne(a => a.UnidadMedida)
                .WithMany()
                .HasForeignKey(a => a.Id_UnidadMedida);

            // Relaciones ArticuloAlmacen
            modelBuilder.Entity<ArticuloAlmacen>()
                .HasOne(aa => aa.Articulo)
                .WithMany()
                .HasForeignKey(aa => aa.Id_Articulo);

            modelBuilder.Entity<ArticuloAlmacen>()
                .HasOne(aa => aa.Almacen)
                .WithMany()
                .HasForeignKey(aa => aa.Id_Almacen);

            // Relaciones DetalleIngreso
            modelBuilder.Entity<DetalleIngreso>()
                .HasOne(di => di.Ingreso)
                .WithMany()
                .HasForeignKey(di => di.Id_Ingreso);

            modelBuilder.Entity<DetalleIngreso>()
                .HasOne(di => di.ArticuloAlmacen)
                .WithMany()
                .HasForeignKey(di => di.Id_ArticuloAlmacen);

            // Relaciones DetalleEgreso
            modelBuilder.Entity<DetalleEgreso>()
                .HasOne(de => de.Egreso)
                .WithMany()
                .HasForeignKey(de => de.Id_Egreso);

            modelBuilder.Entity<DetalleEgreso>()
                .HasOne(de => de.ArticuloAlmacen)
                .WithMany()
                .HasForeignKey(de => de.Id_ArticuloAlmacen);

            // Relaciones Traspaso
            modelBuilder.Entity<Traspaso>()
                .HasOne(t => t.AlmacenOrigen)
                .WithMany()
                .HasForeignKey(t => t.Id_AlmacenOrigen);

            modelBuilder.Entity<Traspaso>()
                .HasOne(t => t.AlmacenDestino)
                .WithMany()
                .HasForeignKey(t => t.Id_AlmacenDestino);

            // Relaciones DetalleTraspaso
            modelBuilder.Entity<DetalleTraspaso>()
                .HasOne(dt => dt.Traspaso)
                .WithMany()
                .HasForeignKey(dt => dt.Id_Traspaso);

            modelBuilder.Entity<DetalleTraspaso>()
                .HasOne(dt => dt.ArticuloAlmacen)
                .WithMany()
                .HasForeignKey(dt => dt.Id_ArticuloAlmacen);
            modelBuilder.Entity<GesPrecio>().ToTable("ges_precio");

            modelBuilder.Entity<GesPrecio>()
                .HasOne(g => g.Articulo)
                .WithMany()
                .HasForeignKey(g => g.Id_Articulo);
        }
    }
}