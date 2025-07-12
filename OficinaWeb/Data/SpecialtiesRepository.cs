using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data.Entities;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public class SpecialtiesRepository : GenericRepository<MechanicSpecialty>, ISpecialtiesRepository
    {
        private readonly DataContext _context;

        public SpecialtiesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

    }
    
    
}
