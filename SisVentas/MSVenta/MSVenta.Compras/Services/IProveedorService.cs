using MSVenta.Compras.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Compras.Services
{
    public interface IProveedorService
    {
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task<Proveedor> GetByIdAsync(int id);
        Task<Proveedor> AddAsync(Proveedor proveedor);
        Task<bool> UpdateAsync(Proveedor proveedor);
        Task<bool> ToggleEstadoAsync(int id);
    }
}