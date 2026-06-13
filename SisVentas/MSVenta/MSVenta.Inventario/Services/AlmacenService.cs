using Microsoft.EntityFrameworkCore;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public class AlmacenService : IAlmacenService
    {
        private readonly ContextDatabase _context;

        public AlmacenService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Almacen>> GetAllAsync()
        {
            return await _context.Almacenes.ToListAsync();
        }

        public async Task<Almacen> GetByIdAsync(int id)
        {
            return await _context.Almacenes
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Almacen> AddAsync(Almacen almacen)
        {
            _context.Almacenes.Add(almacen);
            await _context.SaveChangesAsync();
            return almacen;
        }

        public async Task<bool> UpdateAsync(Almacen almacen)
        {
            var existing = await _context.Almacenes.FindAsync(almacen.Id);
            if (existing == null) return false;

            existing.Nombre = almacen.Nombre;
            existing.Descripcion = almacen.Descripcion;
            existing.Direccion = almacen.Direccion;
            existing.CantidadMax = almacen.CantidadMax;
            existing.Estado = almacen.Estado;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var almacen = await _context.Almacenes.FindAsync(id);
            if (almacen == null) return false;

            _context.Almacenes.Remove(almacen);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}