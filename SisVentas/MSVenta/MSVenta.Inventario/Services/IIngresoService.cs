using MSVenta.Inventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public interface IIngresoService
    {
        Task<IEnumerable<Ingreso>> GetAllAsync();
        Task<Ingreso> GetByIdAsync(int id);
        Task<Ingreso> AddAsync(Ingreso ingreso);
        Task<bool> UpdateAsync(Ingreso ingreso);
        Task<bool> DeleteAsync(int id);
    }
}