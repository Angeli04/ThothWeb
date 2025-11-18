using Thoth.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thoth.Web.Repositories
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> GetAllRolesAsync();
    }
}