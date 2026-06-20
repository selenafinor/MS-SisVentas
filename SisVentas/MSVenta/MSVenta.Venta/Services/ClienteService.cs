using Microsoft.EntityFrameworkCore;
using MSVenta.Venta.Models;
using MSVenta.Venta.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSVenta.Venta.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ContextDatabase _context;

        public ClienteService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientes()
        {
            return await _context.Clientes
                .OrderBy(c => c.Paterno)
                .ThenBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<Cliente> GetCliente(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<IEnumerable<Cliente>> BuscarClientes(string termino)
        {
            termino = termino.ToLower();
            return await _context.Clientes
                .Where(c => c.Estado == "activo" &&
                    (c.Nombre.ToLower().Contains(termino) ||
                     (c.Paterno != null && c.Paterno.ToLower().Contains(termino)) ||
                     (c.Nit != null && c.Nit.Contains(termino))))
                .Take(10)
                .ToListAsync();
        }

        public async Task CreateCliente(Cliente cliente)
        {
            cliente.Estado = "activo";
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCliente(Cliente cliente)
        {
            var existing = await _context.Clientes.FindAsync(cliente.Id);
            if (existing == null) return;

            existing.Nombre = cliente.Nombre;
            existing.Paterno = cliente.Paterno;
            existing.Materno = cliente.Materno;
            existing.Telefono = cliente.Telefono;
            existing.Correo = cliente.Correo;
            existing.Nit = cliente.Nit;
            existing.Direccion = cliente.Direccion;

            await _context.SaveChangesAsync();
        }

        public async Task ToggleEstado(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return;
            cliente.Estado = cliente.Estado == "activo" ? "inactivo" : "activo";
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return;
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}