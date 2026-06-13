using Microsoft.EntityFrameworkCore;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public class EgresoService : IEgresoService
    {
        private readonly ContextDatabase _context;

        public EgresoService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Egreso>> GetAllAsync()
        {
            return await _context.Egresos.ToListAsync();
        }

        public async Task<Egreso> GetByIdAsync(int id)
        {
            return await _context.Egresos
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Egreso> AddAsync(Egreso egreso)
        {
            _context.Egresos.Add(egreso);
            await _context.SaveChangesAsync();
            return egreso;
        }

        public async Task<bool> UpdateAsync(Egreso egreso)
        {
            var existing = await _context.Egresos.FindAsync(egreso.Id);
            if (existing == null) return false;

            existing.Fecha = egreso.Fecha;
            existing.Glosa = egreso.Glosa;
            existing.Motivo = egreso.Motivo;
            existing.Estado = egreso.Estado;
            existing.Id_Usuario = egreso.Id_Usuario;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var egreso = await _context.Egresos.FindAsync(id);
            if (egreso == null) return false;

            _context.Egresos.Remove(egreso);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}