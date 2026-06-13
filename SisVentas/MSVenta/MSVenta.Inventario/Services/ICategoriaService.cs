using MSVenta.Inventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task<Categoria> GetByIdAsync(int id);
        Task<Categoria> AddAsync(Categoria categoria);
        Task<bool> UpdateAsync(Categoria categoria);
        Task<bool> DeleteAsync(int id);
    }
}