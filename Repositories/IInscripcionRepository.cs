using System.Collections.Generic;
using System.Threading.Tasks;
using Thoth.Web.Models;

namespace Thoth.Web.Repositories
{
    public interface IInscripcionRepository
    {
        
        Task<IEnumerable<Inscripcion>> GetInscritosPorCapacitacion(int capacitacionId);

        
        Task<IEnumerable<Usuario>> GetUsuariosDisponiblesParaCurso(int capacitacionId);

        
        Task InscribirUsuario(Inscripcion inscripcion);
        
        Task EliminarInscripcion(int id);
    }
}