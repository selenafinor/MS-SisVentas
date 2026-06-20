using Microsoft.EntityFrameworkCore;
using MSVenta.Compras.Models;
using MSVenta.Compras.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSVenta.Compras.Services
{
    public class NotaCompraService : INotaCompraService
    {
        private readonly ContextDatabase _context;
        private readonly RabbitMqPublisher _publisher;

        public NotaCompraService(ContextDatabase context, RabbitMqPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<IEnumerable<NotaCompra>> GetAllAsync()
        {
            return await _context.NotasCompra
                .Include(c => c.Proveedor)
                .Include(c => c.Detalles)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        public async Task<NotaCompra> GetByIdAsync(int id)
        {
            return await _context.NotasCompra
                .Include(c => c.Proveedor)
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<NotaCompra> CreateAsync(NotaCompra compra)
        {
            compra.FechaCompra = DateTime.Today;
            compra.TotalCompra = 0;
            compra.Estado = "activo";

            _context.NotasCompra.Add(compra);
            await _context.SaveChangesAsync();
            return compra;
        }

        public async Task<DetalleCompra> AgregarDetalleAsync(int idCompra, DetalleCompra detalle)
        {
            var compra = await _context.NotasCompra
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.Id == idCompra);

            if (compra == null)
                throw new ArgumentException("Compra no encontrada.");
            if (compra.Estado == "cancelado")
                throw new ArgumentException("No se puede agregar artículos a una compra cancelada.");

            detalle.CompraId = idCompra;
            detalle.SubTotal = detalle.Cantidad * detalle.PrecioUni;
            _context.DetallesCompra.Add(detalle);
            await _context.SaveChangesAsync();

            compra.TotalCompra = await _context.DetallesCompra
                .Where(d => d.CompraId == idCompra)
                .SumAsync(d => d.SubTotal);
            await _context.SaveChangesAsync();

            return detalle;
        }

        public async Task ConfirmarStockAsync(int idCompra)
        {
            var compra = await _context.NotasCompra
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.Id == idCompra);

            if (compra == null)
                throw new ArgumentException("Compra no encontrada.");
            if (compra.Detalles == null || !compra.Detalles.Any())
                throw new ArgumentException("La compra no tiene artículos.");

            var evento = new CompraRegistradaEvento
            {
                CompraId = compra.Id,
                UsuarioId = compra.UsuarioId ?? 0,
                Detalles = compra.Detalles.Select(d => new DetalleCompraEvento
                {
                    IdProducto = d.ProductoId ?? 0,
                    NombreProducto = d.NombreProducto,
                    Cantidad = d.Cantidad,
                    PrecioUni = d.PrecioUni
                }).ToList()
            };

            _publisher.PublicarCompraRegistrada(evento);
        }

        public async Task<bool> CancelarAsync(int id)
        {
            var compra = await _context.NotasCompra.FindAsync(id);
            if (compra == null) return false;
            if (compra.Estado == "cancelado") return false;

            compra.Estado = "cancelado";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}