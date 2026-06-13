using Microsoft.EntityFrameworkCore;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MSVenta.Inventario.Services
{
    public class DetalleEgresoService : IDetalleEgresoService
    {
        private readonly ContextDatabase _context;

        public DetalleEgresoService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetalleEgreso>> GetAllAsync()
        {
            return await _context.DetallesEgreso.ToListAsync();
        }

        public async Task<DetalleEgreso> GetByIdAsync(int id)
        {
            return await _context.DetallesEgreso
                .FirstOrDefaultAsync(d => d.Id == id);
        }
        public async Task<IEnumerable<DetalleEgreso>> GetByEgresoIdAsync(int egresoId)
        {
            return await _context.DetallesEgreso
                .Where(d => d.Id_Egreso == egresoId)
                .Include(d => d.ArticuloAlmacen)
                    .ThenInclude(aa => aa.Articulo)
                .Include(d => d.ArticuloAlmacen)
                    .ThenInclude(aa => aa.Almacen)
                .ToListAsync();
        }

        public async Task<DetalleEgreso> AddAsync(DetalleEgreso detalle)
        {
            _context.DetallesEgreso.Add(detalle);

            // Disminuir stock en ArticuloAlmacen
            var articuloAlmacen = await _context.ArticulosAlmacenes
                .FirstOrDefaultAsync(aa => aa.Id == detalle.Id_ArticuloAlmacen);

            if (articuloAlmacen != null)
            {
                articuloAlmacen.Stock -= detalle.Cantidad;
                if (articuloAlmacen.Stock < 0) articuloAlmacen.Stock = 0;
            }

            await _context.SaveChangesAsync();
            return detalle;
        }

        public async Task<bool> UpdateAsync(DetalleEgreso detalle)
        {
            var existing = await _context.DetallesEgreso.FindAsync(detalle.Id);
            if (existing == null) return false;
            existing.Cantidad = detalle.Cantidad;
            existing.Observacion = detalle.Observacion;
            existing.Id_Egreso = detalle.Id_Egreso;
            existing.Id_ArticuloAlmacen = detalle.Id_ArticuloAlmacen;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var detalle = await _context.DetallesEgreso.FindAsync(id);
            if (detalle == null) return false;
            _context.DetallesEgreso.Remove(detalle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}