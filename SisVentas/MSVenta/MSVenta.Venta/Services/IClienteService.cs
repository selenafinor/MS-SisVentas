using MSVenta.Venta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Venta.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientes();
        Task<Cliente> GetCliente(int id);
        Task<IEnumerable<Cliente>> BuscarClientes(string termino);
        Task CreateCliente(Cliente cliente);
        Task UpdateCliente(Cliente cliente);
        Task ToggleEstado(int id);
        Task DeleteCliente(int id);
    }
}