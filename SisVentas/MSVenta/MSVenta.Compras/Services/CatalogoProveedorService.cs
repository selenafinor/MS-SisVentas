using Microsoft.EntityFrameworkCore;
using MSVenta.Compras.Models;
using MSVenta.Compras.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSVenta.Compras.Services
{
    public class CatalogoProveedorService : ICatalogoProveedorService
    {
        private readonly ContextDatabase _context;

        public CatalogoProveedorService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CatalogoProveedor>> GetAllAsync()
        {
            return await _context.CatalogoProveedores
                .Include(c => c.Proveedor)
                .ToListAsync();
        }

        public async Task<IEnumerable<CatalogoProveedor>> GetByProveedorIdAsync(int proveedorId)
        {
            return await _context.CatalogoProveedores
                .Where(c => c.ProveedorId == proveedorId)
                .ToListAsync();
        }

        public async Task<CatalogoProveedor> GetByIdAsync(int id)
        {
            return await _context.CatalogoProveedores
                .Include(c => c.Proveedor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CatalogoProveedor> AddAsync(CatalogoProveedor item)
        {
            _context.CatalogoProveedores.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> UpdateAsync(CatalogoProveedor item)
        {
            var existing = await _context.CatalogoProveedores.FindAsync(item.Id);
            if (existing == null) return false;

            existing.PrecioUnitario = item.PrecioUnitario;
            existing.StockDisponible = item.StockDisponible;
            
            existing.Estado = item.Estado;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.CatalogoProveedores.FindAsync(id);
            if (item == null) return false;

            _context.CatalogoProveedores.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<CatalogoProveedor>> GetByProductoIdAsync(int productoId)
        {
            return await _context.CatalogoProveedores
                .Include(c => c.Proveedor)
                .Where(c => c.ProductoId == productoId && c.Estado == "activo")
                .ToListAsync();
        }
    }
}