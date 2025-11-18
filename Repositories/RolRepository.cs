using Microsoft.EntityFrameworkCore;
using Thoth.Web.Data;
using Thoth.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thoth.Web.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly ThothDbContext _context;

        public RolRepository(ThothDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> GetAllRolesAsync()
        {
            // Simplemente lee todos los roles de la tabla
            return await _context.Roles.ToListAsync();
        }
    }
}