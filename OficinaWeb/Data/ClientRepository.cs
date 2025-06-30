using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data.Entities;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Client> GetByEmailAsync(string email)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == email);

            return client;
        }

    }
}
