using MSVenta.Inventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public interface IAlmacenService
    {
        Task<IEnumerable<Almacen>> GetAllAsync();
        Task<Almacen> GetByIdAsync(int id);
        Task<Almacen> AddAsync(Almacen almacen);
        Task<bool> UpdateAsync(Almacen almacen);
        Task<bool> DeleteAsync(int id);
    }
}