using MSVenta.Inventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace MSVenta.Inventario.Services
{
    public interface IDetalleTraspasoService
    {
        Task<IEnumerable<DetalleTraspaso>> GetAllAsync();
        Task<IEnumerable<DetalleTraspaso>> GetByTraspasoIdAsync(int traspasoId);
        Task<DetalleTraspaso> GetByIdAsync(int id);
        Task<DetalleTraspaso> AddAsync(DetalleTraspaso detalle);
        Task<bool> UpdateAsync(DetalleTraspaso detalle);
        Task<bool> DeleteAsync(int id);
    }
}