using Microsoft.EntityFrameworkCore;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public class IngresoService : IIngresoService
    {
        private readonly ContextDatabase _context;

        public IngresoService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingreso>> GetAllAsync()
        {
            return await _context.Ingresos.ToListAsync();
        }

        public async Task<Ingreso> GetByIdAsync(int id)
        {
            return await _context.Ingresos
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Ingreso> AddAsync(Ingreso ingreso)
        {
            _context.Ingresos.Add(ingreso);
            await _context.SaveChangesAsync();
            return ingreso;
        }

        public async Task<bool> UpdateAsync(Ingreso ingreso)
        {
            var existing = await _context.Ingresos.FindAsync(ingreso.Id);
            if (existing == null) return false;

            existing.Fecha = ingreso.Fecha;
            existing.Glosa = ingreso.Glosa;
            existing.Motivo = ingreso.Motivo;
            existing.Estado = ingreso.Estado;
            existing.Id_Usuario = ingreso.Id_Usuario;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso == null) return false;

            _context.Ingresos.Remove(ingreso);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}