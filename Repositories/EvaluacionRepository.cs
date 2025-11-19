using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Thoth.Web.Data;
using Thoth.Web.Models;

namespace Thoth.Web.Repositories
{
    public class EvaluacionRepository : IEvaluacionRepository
    {
        private readonly ThothDbContext _context;

        public EvaluacionRepository(ThothDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Evaluacion evaluacion)
        {
            await _context.Evaluaciones.AddAsync(evaluacion);
            await _context.SaveChangesAsync();
        }

        public async Task<Evaluacion> GetByIdAsync(int id)
        {
            return await _context.Evaluaciones
                                 .Include(e => e.Preguntas)
                                 .ThenInclude(p => p.Opciones)
                                 .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddPreguntaAsync(Pregunta pregunta)
        {
            await _context.Preguntas.AddAsync(pregunta);
            await _context.SaveChangesAsync();
        }






    }
}