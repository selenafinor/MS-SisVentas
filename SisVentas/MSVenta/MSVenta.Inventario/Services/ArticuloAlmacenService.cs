using Microsoft.EntityFrameworkCore;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public class ArticuloAlmacenService : IArticuloAlmacenService
    {
        private readonly ContextDatabase _context;

        public ArticuloAlmacenService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticuloAlmacen>> GetAllAsync()
        {
            return await _context.ArticulosAlmacenes
                .Include(aa => aa.Articulo)
                .Include(aa => aa.Almacen)
                .ToListAsync();
        }

        public async Task<ArticuloAlmacen> GetByIdAsync(int id)
        {
            return await _context.ArticulosAlmacenes
                .Include(aa => aa.Articulo)
                .Include(aa => aa.Almacen)
                .FirstOrDefaultAsync(aa => aa.Id == id);
        }

        public async Task<ArticuloAlmacen> AddAsync(ArticuloAlmacen articuloAlmacen)
        {
            _context.ArticulosAlmacenes.Add(articuloAlmacen);
            await _context.SaveChangesAsync();
            return articuloAlmacen;
        }

        public async Task<bool> UpdateAsync(ArticuloAlmacen articuloAlmacen)
        {
            var existing = await _context.ArticulosAlmacenes
                .FirstOrDefaultAsync(aa => aa.Id == articuloAlmacen.Id);
            if (existing == null) return false;
            existing.Stock = articuloAlmacen.Stock;
            existing.StockMin = articuloAlmacen.StockMin;
            existing.StockMax = articuloAlmacen.StockMax;
            existing.Id_Articulo = articuloAlmacen.Id_Articulo;
            existing.Id_Almacen = articuloAlmacen.Id_Almacen;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var articuloAlmacen = await _context.ArticulosAlmacenes.FindAsync(id);
            if (articuloAlmacen == null) return false;
            _context.ArticulosAlmacenes.Remove(articuloAlmacen);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}