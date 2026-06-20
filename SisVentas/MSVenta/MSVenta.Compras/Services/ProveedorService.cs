using Microsoft.EntityFrameworkCore;
using MSVenta.Compras.Models;
using MSVenta.Compras.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSVenta.Compras.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly ContextDatabase _context;

        public ProveedorService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proveedor>> GetAllAsync()
        {
            return await _context.Proveedores
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<Proveedor> GetByIdAsync(int id)
        {
            return await _context.Proveedores
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Proveedor> AddAsync(Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            return proveedor;
        }

        public async Task<bool> UpdateAsync(Proveedor proveedor)
        {
            var existing = await _context.Proveedores.FindAsync(proveedor.Id);
            if (existing == null) return false;

            existing.Nombre = proveedor.Nombre;
            existing.Telefono = proveedor.Telefono;
            existing.Email = proveedor.Email;
            existing.Direccion = proveedor.Direccion;
            existing.Nit = proveedor.Nit;
            existing.Contacto = proveedor.Contacto;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleEstadoAsync(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return false;

            proveedor.Estado = proveedor.Estado == "activo" ? "inactivo" : "activo";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}