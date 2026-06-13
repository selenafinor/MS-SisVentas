using MSVenta.Inventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public interface IEgresoService
    {
        Task<IEnumerable<Egreso>> GetAllAsync();
        Task<Egreso> GetByIdAsync(int id);
        Task<Egreso> AddAsync(Egreso egreso);
        Task<bool> UpdateAsync(Egreso egreso);
        Task<bool> DeleteAsync(int id);
    }
}