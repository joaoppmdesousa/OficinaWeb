using OficinaWeb.Data.Entities;

namespace OficinaWeb.Data
{
    public class MechanicRepository : GenericRepository<Mechanic>, IMechanicRepository
    {
        public MechanicRepository(DataContext context) : base(context)
        {
        }
    }
}
