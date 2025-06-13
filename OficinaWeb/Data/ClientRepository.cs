using OficinaWeb.Data.Entities;

namespace OficinaWeb.Data
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(DataContext context) : base(context)
        {
        }

    }
}
