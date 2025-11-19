using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq; // Importante para Where y Contains
using System.Threading.Tasks;
using Thoth.Web.Data;
using Thoth.Web.Models;

namespace Thoth.Web.Repositories
{
    public class InscripcionRepository : IInscripcionRepository
    {
        private readonly ThothDbContext _context;

        public InscripcionRepository(ThothDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inscripcion>> GetInscritosPorCapacitacion(int capacitacionId)
        {
            return await _context.Inscripciones
                                 .Include(i => i.Usuario) // Traemos el nombre del empleado
                                 .Where(i => i.CapacitacionId == capacitacionId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosDisponiblesParaCurso(int capacitacionId)
        {
            var idsInscritos = await _context.Inscripciones
                                             .Where(i => i.CapacitacionId == capacitacionId)
                                             .Select(i => i.UsuarioId)
                                             .ToListAsync();

            return await _context.Usuarios
                                 .Where(u => u.EstaActivo && !idsInscritos.Contains(u.Id))
                                 .ToListAsync();
        }

        public async Task InscribirUsuario(Inscripcion inscripcion)
        {
            await _context.Inscripciones.AddAsync(inscripcion);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarInscripcion(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion != null)
            {
                _context.Inscripciones.Remove(inscripcion);
                await _context.SaveChangesAsync();
            }
        }
    }
}