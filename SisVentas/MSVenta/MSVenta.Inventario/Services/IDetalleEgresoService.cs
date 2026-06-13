using MSVenta.Inventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace MSVenta.Inventario.Services
{
    public interface IDetalleEgresoService
    {
        Task<IEnumerable<DetalleEgreso>> GetAllAsync();
        Task<IEnumerable<DetalleEgreso>> GetByEgresoIdAsync(int egresoId);
        Task<DetalleEgreso> GetByIdAsync(int id);
        Task<DetalleEgreso> AddAsync(DetalleEgreso detalle);
        Task<bool> UpdateAsync(DetalleEgreso detalle);
        Task<bool> DeleteAsync(int id);
    }
}