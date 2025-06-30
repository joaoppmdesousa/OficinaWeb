using OficinaWeb.Data.Entities;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public interface IPartsRepository : IGenericRepository<Part>
    {
        public Task<RepairAndServices> GetServiceAsync(int serviceId);
    }
}
