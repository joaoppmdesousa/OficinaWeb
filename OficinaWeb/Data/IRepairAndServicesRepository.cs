using OficinaWeb.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public interface IRepairAndServicesRepository : IGenericRepository<RepairAndServices>
    {
        public  Task AddMechanics(int repairAndServicesId, List<int> mechanicIds);

        public Task<RepairAndServices> GetWithIncludesAsync(int id);

        public Task UpdateMechanicsAsync(int repairId, List<int> newMechanicIds);
    }
}
