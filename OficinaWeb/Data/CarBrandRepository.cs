using OficinaWeb.Data.Entities;

namespace OficinaWeb.Data
{
    public class CarBrandRepository : GenericRepository<CarBrand>, ICarBrandRepository
    {
        private readonly DataContext _context;

        public CarBrandRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }    
}
