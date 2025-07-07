using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data.Entities;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public class AppointmentsRepository : GenericRepository<Appointment>, IAppointmentsRepository
    {
        private readonly DataContext _context;

        public AppointmentsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Appointment> GetByIdAsyncWithIncludes(int id)
        {
            return await _context.Appointment
                .Include(a => a.Client)
                .Include(a => a.Mechanic)
                .Include(a => a.Vehicle)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
