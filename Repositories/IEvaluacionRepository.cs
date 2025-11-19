using System.Threading.Tasks;
using Thoth.Web.Models;

namespace Thoth.Web.Repositories
{
    public interface IEvaluacionRepository
    {
        Task AddAsync(Evaluacion evaluacion);
        Task<Evaluacion> GetByIdAsync(int id);
        Task AddPreguntaAsync(Pregunta pregunta);
        


    }
}