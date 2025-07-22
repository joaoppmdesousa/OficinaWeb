using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data.Entities;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public class PartsRepository : GenericRepository<Part>, IPartsRepository
    {
        private readonly DataContext _context;

        public PartsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public Task<RepairAndServices> GetServiceAsync(int serviceId)
        {
            return _context.RepairsAndServices
                 .AsNoTracking()
                 .FirstOrDefaultAsync(s => s.Id == serviceId);
        }

        public async Task<Part> GetWithIncludesAsync(int id)
        {
            return await _context.Parts
             .Include(p => p.RepairAndServices)
             .FirstOrDefaultAsync(r => r.Id == id);
        }

    }
}
