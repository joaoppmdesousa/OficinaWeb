using Microsoft.AspNetCore.Mvc.Rendering;
using OficinaWeb.Data.Entities;
using System.Collections.Generic;

namespace OficinaWeb.Data
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        IEnumerable<SelectListItem> GetComboClients();
    }
}
