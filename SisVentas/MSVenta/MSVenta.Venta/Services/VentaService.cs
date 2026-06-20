using Microsoft.EntityFrameworkCore;
using MSVenta.Venta.Models;
using MSVenta.Venta.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MSVenta.Venta.Services
{
    public class VentaService : IVentaService
    {
        private readonly ContextDatabase _context;
        private readonly IHttpClientFactory _httpFactory;
        private readonly RabbitMqPublisher _publisher;
        private const string INV_BASE = "http://localhost:5003/api";

        public VentaService(ContextDatabase context, IHttpClientFactory httpFactory, RabbitMqPublisher publisher)
        {
            _context = context;
            _httpFactory = httpFactory;
            _publisher = publisher;
        }

        public async Task<IEnumerable<Models.Venta>> GetAllVentas()
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                .OrderByDescending(v => v.Fecha)
                .ThenByDescending(v => v.Hora)
                .ToListAsync();
        }

        public async Task<Models.Venta> GetVenta(int id)
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task CreateVenta(Models.Venta venta)
        {
            var cliente = await _context.Clientes.FindAsync(venta.ClienteId);
            if (cliente == null)
                throw new ArgumentException("Cliente no encontrado.");

            venta.Cliente = cliente;
            venta.Fecha = DateTime.Today;
            venta.Hora = DateTime.Now.TimeOfDay;
            venta.MontoTotal = 0;

            if (venta.TipoPago == "contado")
            {
                venta.Estado = "pagado";
                venta.PagoConfirmado = true;
            }
            else
            {
                venta.Estado = "activo";
                venta.PagoConfirmado = false;
            }

            await _context.Ventas.AddAsync(venta);
            await _context.SaveChangesAsync();
        }

        public async Task AgregarDetalle(int idVenta, DetalleVenta detalle)
        {
            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(v => v.Id == idVenta);

            if (venta == null)
                throw new ArgumentException("Venta no encontrada.");
            if (venta.Estado == "cancelado")
                throw new ArgumentException("No se puede agregar artículos a una venta cancelada.");

            // Guardar detalle (sin tocar Inventario todavía)
            detalle.VentaId = idVenta;
            detalle.PrecioSubtotal = detalle.Cantidad * detalle.PrecioUni;
            _context.DetallesVenta.Add(detalle);
            await _context.SaveChangesAsync();

            // Recalcular monto total
            venta.MontoTotal = await _context.DetallesVenta
                .Where(d => d.VentaId == idVenta)
                .SumAsync(d => d.PrecioSubtotal);
            await _context.SaveChangesAsync();
        }
        public async Task ConfirmarStock(int idVenta)
        {
            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(v => v.Id == idVenta);

            if (venta == null)
                throw new ArgumentException("Venta no encontrada.");
            if (venta.Detalles == null || !venta.Detalles.Any())
                throw new ArgumentException("La venta no tiene artículos.");

            var evento = new VentaCreadaEvento
            {
                VentaId = venta.Id,
                UsuarioId = venta.UsuarioId,
                Detalles = venta.Detalles.Select(d => new DetalleVentaEvento
                {
                    IdProducto = d.Id_Producto,
                    NombreProducto = d.NombreProducto,
                    IdAlmacen = d.Id_Almacen,
                    Cantidad = d.Cantidad,
                    PrecioUni = d.PrecioUni
                }).ToList()
            };

            _publisher.PublicarVentaCreada(evento);
        }

        public async Task CancelarVenta(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null) throw new ArgumentException("Venta no encontrada.");
            if (venta.Estado == "cancelado") throw new ArgumentException("La venta ya está cancelada.");

            venta.Estado = "cancelado";
            var http = _httpFactory.CreateClient();

            var ingresoPayload = JsonSerializer.Serialize(new
            {
                fecha = DateTime.Today,
                glosa = $"Devolución por cancelación de venta #{id}",
                motivo = "devolucion",
                estado = "activo",
                id_Usuario = venta.UsuarioId
            });
            var ingresoResp = await http.PostAsync($"{INV_BASE}/ingreso",
                new StringContent(ingresoPayload, Encoding.UTF8, "application/json"));

            int? ingresoId = null;
            if (ingresoResp.IsSuccessStatusCode)
            {
                var ingresoJson = await ingresoResp.Content.ReadAsStringAsync();
                var ingreso = JsonSerializer.Deserialize<IdDto>(ingresoJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                ingresoId = ingreso?.Id;
            }

            foreach (var detalle in venta.Detalles)
            {
                var artResp = await http.GetAsync(
                    $"{INV_BASE}/ArticuloAlmacen/{detalle.Id_Producto}");

                if (artResp.IsSuccessStatusCode)
                {
                    var artJson = await artResp.Content.ReadAsStringAsync();
                    var art = JsonSerializer.Deserialize<ArticuloAlmacenDto>(artJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    var stockPayload = JsonSerializer.Serialize(new
                    {
                        id = art.Id,
                        stock = art.Stock + (int)detalle.Cantidad,
                        stockMin = art.StockMin,
                        stockMax = art.StockMax,
                        id_Articulo = art.Id_Articulo,
                        id_Almacen = art.Id_Almacen
                    });
                    await http.PutAsync($"{INV_BASE}/ArticuloAlmacen/{art.Id}",
                        new StringContent(stockPayload, Encoding.UTF8, "application/json"));

                    if (ingresoId.HasValue)
                    {
                        var detalleIngresoPayload = JsonSerializer.Serialize(new
                        {
                            cantidad = (int)detalle.Cantidad,
                            precioCompra = detalle.PrecioUni,
                            observacion = $"Devolución - Venta #{id}",
                            id_Ingreso = ingresoId.Value,
                            id_ArticuloAlmacen = art.Id
                        });
                        await http.PostAsync($"{INV_BASE}/DetalleIngreso",
                            new StringContent(detalleIngresoPayload, Encoding.UTF8, "application/json"));
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task ConfirmarPago(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) throw new ArgumentException("Venta no encontrada.");
            venta.Estado = "pagado";
            venta.PagoConfirmado = true;
            await _context.SaveChangesAsync();
        }
        public async Task AsignarTransaccionQr(int idVenta, string idTransaccion)
        {
            var venta = await _context.Ventas.FindAsync(idVenta);
            if (venta != null)
            {
                venta.IdTransaccionQr = idTransaccion;
                venta.TipoPago = "qr";
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateVenta(Models.Venta venta)
        {
            _context.Entry(venta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVenta(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return;
            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
        }
    }

    internal class ArticuloAlmacenDto
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
        public int Id_Articulo { get; set; }
        public int Id_Almacen { get; set; }
    }

    internal class IdDto
    {
        public int Id { get; set; }
    }
}