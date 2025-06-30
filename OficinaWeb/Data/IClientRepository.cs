using OficinaWeb.Data.Entities;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client> GetByEmailAsync(string email);

       
    }
}
