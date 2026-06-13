using Microsoft.EntityFrameworkCore;
using MSVenta.Inventario.DTOs;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public class ArticuloService : IArticuloService
    {
        private readonly ContextDatabase _context;

        public ArticuloService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticuloDto>> GetAllAsync()
        {
            var articulos = await _context.Articulos
                .Include(a => a.Marca)
                .Include(a => a.Categoria)
                .Include(a => a.UnidadMedida)
                .ToListAsync();

            var articulosAlmacen = await _context.ArticulosAlmacenes.ToListAsync();

            return articulos.Select(a =>
            {
                var aas = articulosAlmacen.Where(x => x.Id_Articulo == a.Id).ToList();
                var stockTotal = aas.Sum(x => x.Stock);
                var aa = aas.FirstOrDefault();
                return new ArticuloDto
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Descripcion = a.Descripcion,
                    Precio = a.Precio,
                    Estado = a.Estado,
                    Foto = a.Foto,
                    Id_Marca = a.Id_Marca,
                    NombreMarca = a.Marca?.Nombre,
                    Id_Categoria = a.Id_Categoria,
                    NombreCategoria = a.Categoria?.Nombre,
                    Id_UnidadMedida = a.Id_UnidadMedida,
                    NombreUnidadMedida = a.UnidadMedida?.Nombre,
                    Stock = stockTotal,
                    StockMin = aa?.StockMin ?? 0,
                    StockMax = aa?.StockMax ?? 0,
                };
            });
        }

        public async Task<Articulo> GetByIdAsync(int id)
        {
            return await _context.Articulos
                .Include(a => a.Marca)
                .Include(a => a.Categoria)
                .Include(a => a.UnidadMedida)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Articulo> AddAsync(Articulo articulo)
        {
            _context.Articulos.Add(articulo);
            await _context.SaveChangesAsync();
            return articulo;
        }

        public async Task<bool> UpdateAsync(Articulo articulo)
        {
            var existing = await _context.Articulos.FindAsync(articulo.Id);
            if (existing == null) return false;
            existing.Nombre = articulo.Nombre;
            existing.Descripcion = articulo.Descripcion;
            existing.Precio = articulo.Precio;
            existing.Estado = articulo.Estado;
            existing.Id_Marca = articulo.Id_Marca;
            existing.Id_Categoria = articulo.Id_Categoria;
            existing.Id_UnidadMedida = articulo.Id_UnidadMedida;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateFotoAsync(int id, string fotoUrl)
        {
            var existing = await _context.Articulos.FindAsync(id);
            if (existing == null) return false;
            existing.Foto = fotoUrl;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null) return false;
            _context.Articulos.Remove(articulo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}