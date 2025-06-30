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

    }
}
