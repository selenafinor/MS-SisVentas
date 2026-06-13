using Microsoft.EntityFrameworkCore;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public class DetalleIngresoService : IDetalleIngresoService
    {
        private readonly ContextDatabase _context;

        public DetalleIngresoService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetalleIngreso>> GetAllAsync()
        {
            return await _context.DetallesIngreso.ToListAsync();
        }

        public async Task<IEnumerable<DetalleIngreso>> GetByIngresoIdAsync(int ingresoId)
        {
            return await _context.DetallesIngreso
                .Where(d => d.Id_Ingreso == ingresoId)
                .Include(d => d.ArticuloAlmacen)
                    .ThenInclude(aa => aa.Articulo)
                .Include(d => d.ArticuloAlmacen)
                    .ThenInclude(aa => aa.Almacen)
                .ToListAsync();
        }

        public async Task<DetalleIngreso> GetByIdAsync(int id)
        {
            return await _context.DetallesIngreso
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DetalleIngreso> AddAsync(DetalleIngreso detalle)
        {
            _context.DetallesIngreso.Add(detalle);

            // Actualizar stock en ArticuloAlmacen
            var articuloAlmacen = await _context.ArticulosAlmacenes
                .FirstOrDefaultAsync(aa => aa.Id == detalle.Id_ArticuloAlmacen);

            if (articuloAlmacen != null)
            {
                articuloAlmacen.Stock += detalle.Cantidad;
            }

            await _context.SaveChangesAsync();
            return detalle;
        }

        public async Task<bool> UpdateAsync(DetalleIngreso detalle)
        {
            var existing = await _context.DetallesIngreso.FindAsync(detalle.Id);
            if (existing == null) return false;
            existing.Cantidad = detalle.Cantidad;
            existing.PrecioCompra = detalle.PrecioCompra;
            existing.Observacion = detalle.Observacion;
            existing.Id_Ingreso = detalle.Id_Ingreso;
            existing.Id_ArticuloAlmacen = detalle.Id_ArticuloAlmacen;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var detalle = await _context.DetallesIngreso.FindAsync(id);
            if (detalle == null) return false;
            _context.DetallesIngreso.Remove(detalle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}