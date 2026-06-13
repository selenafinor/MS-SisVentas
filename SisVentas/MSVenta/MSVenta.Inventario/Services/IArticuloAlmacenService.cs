using MSVenta.Inventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public interface IArticuloAlmacenService
    {
        Task<IEnumerable<ArticuloAlmacen>> GetAllAsync();
        Task<ArticuloAlmacen> GetByIdAsync(int id);
        Task<ArticuloAlmacen> AddAsync(ArticuloAlmacen articuloAlmacen);
        Task<bool> UpdateAsync(ArticuloAlmacen articuloAlmacen);
        Task<bool> DeleteAsync(int id);
    }
}