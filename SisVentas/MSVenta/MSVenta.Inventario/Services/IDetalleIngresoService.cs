using MSVenta.Inventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public interface IDetalleIngresoService
    {
        Task<IEnumerable<DetalleIngreso>> GetAllAsync();
        Task<IEnumerable<DetalleIngreso>> GetByIngresoIdAsync(int ingresoId);
        Task<DetalleIngreso> GetByIdAsync(int id);
        Task<DetalleIngreso> AddAsync(DetalleIngreso detalle);
        Task<bool> UpdateAsync(DetalleIngreso detalle);
        Task<bool> DeleteAsync(int id);
    }
}