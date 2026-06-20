using MSVenta.Venta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Venta.Services
{
    public interface IVentaService
    {
        Task<IEnumerable<Models.Venta>> GetAllVentas();
        Task<Models.Venta> GetVenta(int id);
        Task CreateVenta(Models.Venta venta);
        Task AgregarDetalle(int idVenta, DetalleVenta detalle);
        Task ConfirmarStock(int idVenta);
        Task CancelarVenta(int id);
        Task ConfirmarPago(int id);
        Task UpdateVenta(Models.Venta venta);
        Task DeleteVenta(int id);
        Task AsignarTransaccionQr(int idVenta, string idTransaccion);

    }
}