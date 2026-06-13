using Microsoft.EntityFrameworkCore;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MSVenta.Inventario.Services
{
    public class DetalleTraspasoService : IDetalleTraspasoService
    {
        private readonly ContextDatabase _context;

        public DetalleTraspasoService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetalleTraspaso>> GetAllAsync()
        {
            return await _context.DetallesTraspaso.ToListAsync();
        }

        public async Task<DetalleTraspaso> GetByIdAsync(int id)
        {
            return await _context.DetallesTraspaso
                .FirstOrDefaultAsync(d => d.Id == id);
        }
        public async Task<IEnumerable<DetalleTraspaso>> GetByTraspasoIdAsync(int traspasoId)
        {
            return await _context.DetallesTraspaso
                .Where(d => d.Id_Traspaso == traspasoId)
                .Include(d => d.ArticuloAlmacen)
                    .ThenInclude(aa => aa.Articulo)
                .Include(d => d.ArticuloAlmacen)
                    .ThenInclude(aa => aa.Almacen)
                .ToListAsync();
        }

        public async Task<DetalleTraspaso> AddAsync(DetalleTraspaso detalle)
        {
            _context.DetallesTraspaso.Add(detalle);

            // Obtener el traspaso para saber origen y destino
            var traspaso = await _context.Traspasos
                .FirstOrDefaultAsync(t => t.Id == detalle.Id_Traspaso);

            if (traspaso != null)
            {
                // Disminuir stock en almacén origen
                var aaOrigen = await _context.ArticulosAlmacenes
                    .FirstOrDefaultAsync(aa => aa.Id == detalle.Id_ArticuloAlmacen);

                if (aaOrigen != null)
                {
                    aaOrigen.Stock -= detalle.Cantidad;
                    if (aaOrigen.Stock < 0) aaOrigen.Stock = 0;

                    // Aumentar stock en almacén destino
                    var aaDestino = await _context.ArticulosAlmacenes
                        .FirstOrDefaultAsync(aa =>
                            aa.Id_Articulo == aaOrigen.Id_Articulo &&
                            aa.Id_Almacen == traspaso.Id_AlmacenDestino);

                    if (aaDestino != null)
                    {
                        aaDestino.Stock += detalle.Cantidad;
                    }
                    else
                    {
                        // Si no existe el registro en el almacén destino, lo creamos
                        var nuevoAA = new ArticuloAlmacen
                        {
                            Stock = detalle.Cantidad,
                            StockMin = aaOrigen.StockMin,
                            StockMax = aaOrigen.StockMax,
                            Id_Articulo = aaOrigen.Id_Articulo,
                            Id_Almacen = traspaso.Id_AlmacenDestino
                        };
                        _context.ArticulosAlmacenes.Add(nuevoAA);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return detalle;
        }

        public async Task<bool> UpdateAsync(DetalleTraspaso detalle)
        {
            var existing = await _context.DetallesTraspaso.FindAsync(detalle.Id);
            if (existing == null) return false;
            existing.Cantidad = detalle.Cantidad;
            existing.Id_Traspaso = detalle.Id_Traspaso;
            existing.Id_ArticuloAlmacen = detalle.Id_ArticuloAlmacen;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var detalle = await _context.DetallesTraspaso.FindAsync(id);
            if (detalle == null) return false;
            _context.DetallesTraspaso.Remove(detalle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}