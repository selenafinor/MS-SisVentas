using Microsoft.EntityFrameworkCore;
using MSVenta.Compras.Models;
using MSVenta.Compras.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MSVenta.Compras.Services
{
    public class OrdenCompraService : IOrdenCompraService
    {
        private readonly ContextDatabase _context;
        public OrdenCompraService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrdenCompra>> GetAllAsync()
        {
            return await _context.OrdenesCompra
                .Include(o => o.Proveedor)
                .Include(o => o.Detalles)
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();
        }

        public async Task<OrdenCompra> GetByIdAsync(int id)
        {
            return await _context.OrdenesCompra
                .Include(o => o.Proveedor)
                .Include(o => o.Detalles)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<OrdenCompra> CreateAsync(OrdenCompra orden)
        {
            orden.Fecha = DateTime.Today;
            orden.Estado = "pendiente";
            orden.Total = 0;
            _context.OrdenesCompra.Add(orden);
            await _context.SaveChangesAsync();
            return orden;
        }

        public async Task<DetalleOrdenCompra> AgregarDetalleAsync(int idOrden, DetalleOrdenCompra detalle)
        {
            var orden = await _context.OrdenesCompra
                .Include(o => o.Detalles)
                .FirstOrDefaultAsync(o => o.Id == idOrden);
            if (orden == null)
                throw new ArgumentException("Orden no encontrada.");
            if (orden.Estado != "pendiente")
                throw new ArgumentException("Solo se pueden agregar artículos a una orden pendiente.");

            detalle.OrdenId = idOrden;
            detalle.SubTotal = detalle.Cantidad * detalle.PrecioUni;
            _context.DetallesOrdenCompra.Add(detalle);
            await _context.SaveChangesAsync();

            await RecalcularTotalAsync(idOrden);
            return detalle;
        }

        public async Task<DetalleOrdenCompra> ActualizarDetalleAsync(int idDetalle, decimal? cantidad, decimal? precioUni)
        {
            var detalle = await _context.DetallesOrdenCompra.FindAsync(idDetalle);
            if (detalle == null)
                throw new ArgumentException("Detalle no encontrado.");

            if (cantidad.HasValue) detalle.Cantidad = cantidad.Value;
            if (precioUni.HasValue) detalle.PrecioUni = precioUni.Value;
            detalle.SubTotal = detalle.Cantidad * detalle.PrecioUni;
            await _context.SaveChangesAsync();

            await RecalcularTotalAsync(detalle.OrdenId);
            return detalle;
        }

        public async Task<bool> EliminarDetalleAsync(int idDetalle)
        {
            var detalle = await _context.DetallesOrdenCompra.FindAsync(idDetalle);
            if (detalle == null) return false;

            int idOrden = detalle.OrdenId;
            _context.DetallesOrdenCompra.Remove(detalle);
            await _context.SaveChangesAsync();

            await RecalcularTotalAsync(idOrden);
            return true;
        }

        public async Task<bool> ActualizarEstadoAsync(int idOrden, string estado)
        {
            var orden = await _context.OrdenesCompra.FindAsync(idOrden);
            if (orden == null) return false;
            orden.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task RecalcularTotalAsync(int idOrden)
        {
            var orden = await _context.OrdenesCompra.FindAsync(idOrden);
            orden.Total = await _context.DetallesOrdenCompra
                .Where(d => d.OrdenId == idOrden)
                .SumAsync(d => d.SubTotal);
            await _context.SaveChangesAsync();
        }
    }
}