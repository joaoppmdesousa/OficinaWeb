using OficinaWeb.Data.Entities;

namespace OficinaWeb.Data
{
    public class ServiceTypeRepository : GenericRepository<ServiceType>, IServiceTypeRepository
    {
        private readonly DataContext _context;

        public ServiceTypeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

    }
}
