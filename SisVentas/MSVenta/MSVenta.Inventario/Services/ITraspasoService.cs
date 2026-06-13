using MSVenta.Inventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public interface ITraspasoService
    {
        Task<IEnumerable<Traspaso>> GetAllAsync();
        Task<Traspaso> GetByIdAsync(int id);
        Task<Traspaso> AddAsync(Traspaso traspaso);
        Task<bool> UpdateAsync(Traspaso traspaso);
        Task<bool> DeleteAsync(int id);
    }
}