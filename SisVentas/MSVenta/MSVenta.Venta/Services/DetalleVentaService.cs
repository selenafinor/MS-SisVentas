using Microsoft.EntityFrameworkCore;
using MSVenta.Venta.Models;
using MSVenta.Venta.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSVenta.Venta.Services
{
    public class DetalleVentaService : IDetalleVentaService
    {
        private readonly ContextDatabase _context;

        public DetalleVentaService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetalleVenta>> GetAllDetalles()
        {
            return await _context.DetallesVenta
                .Include(dv => dv.Venta)
                .ToListAsync();
        }

        public async Task<DetalleVenta> GetDetalle(int id)
        {
            return await _context.DetallesVenta
                .Include(dv => dv.Venta)
                .FirstOrDefaultAsync(dv => dv.Id == id);
        }

        public async Task CreateDetalle(DetalleVenta detalle)
        {
            _context.DetallesVenta.Add(detalle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDetalle(DetalleVenta detalle)
        {
            var existing = await _context.DetallesVenta.FindAsync(detalle.Id);
            if (existing == null) return;

            existing.Cantidad = detalle.Cantidad;
            existing.PrecioUni = detalle.PrecioUni;
            existing.PrecioSubtotal = detalle.PrecioSubtotal;
            existing.VentaId = detalle.VentaId;
            existing.Id_Producto = detalle.Id_Producto;
            existing.Id_Almacen = detalle.Id_Almacen;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDetalle(int id)
        {
            var detalle = await _context.DetallesVenta.FindAsync(id);
            if (detalle == null) return;

            _context.DetallesVenta.Remove(detalle);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DetalleVenta>> GetDetallesPorVenta(int ventaId)
        {
            return await _context.DetallesVenta
                .Include(dv => dv.Venta)
                .Where(dv => dv.VentaId == ventaId)
                .ToListAsync();
        }
    }
}