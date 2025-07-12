using OficinaWeb.Data.Entities;

namespace OficinaWeb.Data
{
    public class CarModelRepository : GenericRepository<CarModel>, ICarModelRepository
    {
        private readonly DataContext _context;
        public CarModelRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
