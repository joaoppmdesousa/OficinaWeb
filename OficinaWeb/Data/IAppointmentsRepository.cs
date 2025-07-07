using OficinaWeb.Data.Entities;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public interface IAppointmentsRepository : IGenericRepository<Appointment>
    {
        public Task<Appointment> GetByIdAsyncWithIncludes(int id);

    }
}
