using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OficinaWeb.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {   
        public Task<Client> GetClientAsync(int clientId);

        public Task<Vehicle> GetByIdAsyncWithIncludes(int id);

    }
}
