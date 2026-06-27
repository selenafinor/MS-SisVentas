using Microsoft.EntityFrameworkCore;
using MSVenta.Compras.Models;
using MSVenta.Compras.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MSVenta.Compras.Services
{
    public class AdquisicionService : IAdquisicionService
    {
        private readonly ContextDatabase _context;
        private readonly RabbitMqPublisher _publisher;
        public AdquisicionService(ContextDatabase context, RabbitMqPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<IEnumerable<Adquisicion>> GetAllAsync()
        {
            return await _context.Adquisiciones
                .Include(a => a.Proveedor)
                .Include(a => a.Orden)
                .Include(a => a.Detalles)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<Adquisicion> GetByIdAsync(int id)
        {
            return await _context.Adquisiciones
                .Include(a => a.Proveedor)
                .Include(a => a.Orden)
                .Include(a => a.Detalles)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Adquisicion> CreateAsync(Adquisicion adquisicion)
        {
            adquisicion.Fecha = DateTime.Today;
            adquisicion.Estado = "activo";
            _context.Adquisiciones.Add(adquisicion);
            await _context.SaveChangesAsync();
            return adquisicion;
        }

        public async Task<DetalleAdquisicion> AgregarDetalleAsync(int idAdquisicion, DetalleAdquisicion detalle)
        {
            var adquisicion = await _context.Adquisiciones.FindAsync(idAdquisicion);
            if (adquisicion == null)
                throw new ArgumentException("Adquisición no encontrada.");

            detalle.AdquisicionId = idAdquisicion;
            detalle.SubTotal = detalle.Cantidad * detalle.PrecioUni;
            _context.DetallesAdquisicion.Add(detalle);
            await _context.SaveChangesAsync();
            return detalle;
        }

        public async Task ConfirmarStockAsync(int idAdquisicion)
        {
            var adquisicion = await _context.Adquisiciones
                .Include(a => a.Detalles)
                .FirstOrDefaultAsync(a => a.Id == idAdquisicion);
            if (adquisicion == null)
                throw new ArgumentException("Adquisición no encontrada.");
            if (adquisicion.Detalles == null || !adquisicion.Detalles.Any())
                throw new ArgumentException("La adquisición no tiene artículos.");

            var evento = new AdquisicionRegistradaEvento
            {
                AdquisicionId = adquisicion.Id,
                UsuarioId = adquisicion.UsuarioId ?? 0,
                Detalles = adquisicion.Detalles.Select(d => new DetalleAdquisicionEvento
                {
                    IdProducto = d.ProductoId ?? 0,
                    IdAlmacen = d.AlmacenId ?? 0,
                    NombreProducto = d.NombreProducto,
                    Cantidad = d.Cantidad,
                    PrecioUni = d.PrecioUni
                }).ToList()
            };
            _publisher.PublicarAdquisicionRegistrada(evento);
        }
    }
}