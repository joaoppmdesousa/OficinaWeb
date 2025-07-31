using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly DataContext _context;

        public VehicleRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public Task<Client> GetClientAsync(int clientId)
        {
           return _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clientId);
        }

        public async Task<Vehicle> GetByIdAsyncWithIncludes(int id)
        {
            return await _context.Vehicles
                .Include(a => a.CarBrand)
                .Include(a => a.CarModel)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task<bool> CheckLicenseExists(string newLicensePlate, int? excludeVehicleId)
        {
            return await _context.Vehicles
                .AnyAsync(v => v.LicensePlate == newLicensePlate &&
                              (!excludeVehicleId.HasValue || v.Id != excludeVehicleId.Value));
        }


    }
}
