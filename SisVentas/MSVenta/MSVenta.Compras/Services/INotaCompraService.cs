using MSVenta.Compras.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Compras.Services
{
    public interface INotaCompraService
    {
        Task<IEnumerable<NotaCompra>> GetAllAsync();
        Task<NotaCompra> GetByIdAsync(int id);
        Task<NotaCompra> CreateAsync(NotaCompra compra);
        Task<DetalleCompra> AgregarDetalleAsync(int idCompra, DetalleCompra detalle);
        Task ConfirmarStockAsync(int idCompra);
        Task<bool> CancelarAsync(int id);
    }
}