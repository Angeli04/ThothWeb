using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thoth.Web.Data;
using Thoth.Web.Models;

namespace Thoth.Web.Repositories
{
    public class CapacitacionRepository : ICapacitacionRepository
    {
        private readonly ThothDbContext _context;

        public CapacitacionRepository(ThothDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Capacitacion>> GetAllAsync()
        {
            return await _context.Capacitaciones
                                 .OrderByDescending(c => c.FechaCreacion)
                                 .ToListAsync();
        }

        public async Task<Capacitacion> GetByIdAsync(int id)
        {
            return await _context.Capacitaciones
                                .Include(c => c.Evaluacion)
                                    .ThenInclude(e => e.Preguntas)
                                        .ThenInclude(p => p.Opciones)
                                        .Include(c => c.Inscripciones)
                                            .ThenInclude(i => i.Usuario)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Capacitacion capacitacion)
        {
            await _context.Capacitaciones.AddAsync(capacitacion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Capacitacion capacitacion)
        {
            _context.Capacitaciones.Update(capacitacion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Capacitaciones.FindAsync(id);
            if (entity != null)
            {
                _context.Capacitaciones.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}