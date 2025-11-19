using System.Collections.Generic;
using System.Threading.Tasks;
using Thoth.Web.Models;

namespace Thoth.Web.Repositories
{
    public interface ICapacitacionRepository
    {
        // Leer
        Task<IEnumerable<Capacitacion>> GetAllAsync();
        Task<Capacitacion> GetByIdAsync(int id);

        // Escribir
        Task AddAsync(Capacitacion capacitacion);
        Task UpdateAsync(Capacitacion capacitacion);
        Task DeleteAsync(int id);
    }
}