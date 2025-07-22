using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<Mechanic> GetByIdAsyncWithIncludes(int id)
        {
            return await _context.Mechanics
               .Include(a => a.MechanicSpecialty)
               .AsNoTracking()
               .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Mechanic>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Mechanics
             .Where(m => ids.Contains(m.Id))
            .ToListAsync();
        }


        public async Task<Mechanic> GetByEmailAsync(string email)
        {
            var mechanic = await _context.Mechanics.FirstOrDefaultAsync(c => c.Email == email);

            return mechanic;
        }

        public IEnumerable<SelectListItem> GetComboProducts()
        {
            var list = _context.Mechanics.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();         

            return list;
        }

    }
}
