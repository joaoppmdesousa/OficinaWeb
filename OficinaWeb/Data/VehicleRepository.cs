using Microsoft.AspNetCore.Mvc.Rendering;
using OficinaWeb.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OficinaWeb.Data
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly DataContext _context;

        public VehicleRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboClients()
        {
            var list = _context.Clients.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a client...)",
                Value = "0"
            });

            return list;
        }

    }
}
