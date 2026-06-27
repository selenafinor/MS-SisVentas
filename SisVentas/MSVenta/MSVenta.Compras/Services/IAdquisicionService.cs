using MSVenta.Compras.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace MSVenta.Compras.Services
{
    public interface IAdquisicionService
    {
        Task<IEnumerable<Adquisicion>> GetAllAsync();
        Task<Adquisicion> GetByIdAsync(int id);
        Task<Adquisicion> CreateAsync(Adquisicion adquisicion);
        Task<DetalleAdquisicion> AgregarDetalleAsync(int idAdquisicion, DetalleAdquisicion detalle);
        Task ConfirmarStockAsync(int idAdquisicion);
    }
}