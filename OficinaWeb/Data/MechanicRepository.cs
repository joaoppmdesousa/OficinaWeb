using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public class MechanicRepository : GenericRepository<Mechanic>, IMechanicRepository
    {
        private readonly DataContext _context;

        public MechanicRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Mechanic>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Mechanics
             .Where(m => ids.Contains(m.Id))
            .ToListAsync();
        }

    

    }
}
