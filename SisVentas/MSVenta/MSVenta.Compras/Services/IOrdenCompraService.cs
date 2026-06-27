using MSVenta.Compras.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace MSVenta.Compras.Services
{
    public interface IOrdenCompraService
    {
        Task<IEnumerable<OrdenCompra>> GetAllAsync();
        Task<OrdenCompra> GetByIdAsync(int id);
        Task<OrdenCompra> CreateAsync(OrdenCompra orden);
        Task<DetalleOrdenCompra> AgregarDetalleAsync(int idOrden, DetalleOrdenCompra detalle);
        Task<DetalleOrdenCompra> ActualizarDetalleAsync(int idDetalle, decimal? cantidad, decimal? precioUni);
        Task<bool> EliminarDetalleAsync(int idDetalle);
        Task<bool> ActualizarEstadoAsync(int idOrden, string estado);
    }
}