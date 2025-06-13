using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using OficinaWeb.Data.Entities;



namespace OficinaWeb.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }


        public IEnumerable<Client> GetClients()
        {
            return _context.Clients.OrderBy(c => c.Name);
        }

        public Client GetClient(int id)
        {
            return _context.Clients.Find(id);
        }

        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
        }

        public void UpdateClient(Client client)
        {
            _context.Clients.Update(client);
        }

        public void RemoveClient(Client client)
        { 
             _context.Clients.Remove(client);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool ClientExists(int id)
        {
            return _context.Clients.Any(c => c.Id == id);
        }
    }
}
