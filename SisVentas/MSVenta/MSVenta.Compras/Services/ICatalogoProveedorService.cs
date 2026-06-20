using MSVenta.Compras.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Compras.Services
{
    public interface ICatalogoProveedorService
    {
        Task<IEnumerable<CatalogoProveedor>> GetAllAsync();
        Task<IEnumerable<CatalogoProveedor>> GetByProveedorIdAsync(int proveedorId);
        Task<CatalogoProveedor> GetByIdAsync(int id);
        Task<CatalogoProveedor> AddAsync(CatalogoProveedor item);
        Task<bool> UpdateAsync(CatalogoProveedor item);
        Task<bool> DeleteAsync(int id);
    }
}