using Microsoft.EntityFrameworkCore;
using MSVenta.Inventario.Models;
using MSVenta.Inventario.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSVenta.Inventario.Services
{
    public class TraspasoService : ITraspasoService
    {
        private readonly ContextDatabase _context;

        public TraspasoService(ContextDatabase context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Traspaso>> GetAllAsync()
        {
            return await _context.Traspasos
                .Include(t => t.AlmacenOrigen)
                .Include(t => t.AlmacenDestino)
                .ToListAsync();
        }

        public async Task<Traspaso> GetByIdAsync(int id)
        {
            return await _context.Traspasos
                .Include(t => t.AlmacenOrigen)
                .Include(t => t.AlmacenDestino)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Traspaso> AddAsync(Traspaso traspaso)
        {
            _context.Traspasos.Add(traspaso);
            await _context.SaveChangesAsync();
            return traspaso;
        }

        public async Task<bool> UpdateAsync(Traspaso traspaso)
        {
            var existing = await _context.Traspasos.FindAsync(traspaso.Id);
            if (existing == null) return false;

            existing.Fecha = traspaso.Fecha;
            existing.Glosa = traspaso.Glosa;
            existing.Estado = traspaso.Estado;
            existing.Id_Usuario = traspaso.Id_Usuario;
            existing.Id_AlmacenOrigen = traspaso.Id_AlmacenOrigen;
            existing.Id_AlmacenDestino = traspaso.Id_AlmacenDestino;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var traspaso = await _context.Traspasos.FindAsync(id);
            if (traspaso == null) return false;

            _context.Traspasos.Remove(traspaso);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}