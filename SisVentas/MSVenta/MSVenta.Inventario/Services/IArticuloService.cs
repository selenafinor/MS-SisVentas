using MSVenta.Inventario.DTOs;
using MSVenta.Inventario.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public interface IArticuloService
    {
        Task<IEnumerable<ArticuloDto>> GetAllAsync();
        Task<Articulo> GetByIdAsync(int id);
        Task<Articulo> AddAsync(Articulo articulo);
        Task<bool> UpdateAsync(Articulo articulo);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateFotoAsync(int id, string fotoUrl);
    }
}