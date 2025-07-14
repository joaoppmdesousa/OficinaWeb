using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public class RepairAndServicesRepository : GenericRepository<RepairAndServices>, IRepairAndServicesRepository
    {
        private readonly DataContext _context;

        public RepairAndServicesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddMechanics(int repairAndServicesId, List<int> mechanicIds)
        {
           
            var repair = await _context.RepairsAndServices
                .Include(r => r.Mechanics)
                .FirstOrDefaultAsync(r => r.Id == repairAndServicesId);

            if (repair == null)
                throw new Exception("Repair not found");

            
            var mechanics = await _context.Mechanics
                .Where(m => mechanicIds.Contains(m.Id))
                .ToListAsync();           

           
            foreach (var mechanic in mechanics)
            {
                if (!repair.Mechanics.Any(m => m.Id == mechanic.Id))
                {
                    repair.Mechanics.Add(mechanic);
                }
            }

            

           
            await _context.SaveChangesAsync();
        }


        public async Task<RepairAndServices> GetWithIncludesAsync(int id)
        {
            return await _context.RepairsAndServices
             .Include(r => r.Client)
             .Include(r => r.Vehicle)
             .ThenInclude(v => v.CarBrand)
             .Include(r => r.Vehicle)
             .ThenInclude(v => v.CarModel)
             .Include(r => r.Mechanics)
             .Include(r => r.Parts)
             .Include(r => r.ServiceType)
             .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateMechanicsAsync(int repairId, List<int> newMechanicIds)
        {
            var repair = await _context.RepairsAndServices
                .Include(r => r.Mechanics)
                .FirstOrDefaultAsync(r => r.Id == repairId);

            if (repair == null)
            {
                throw new Exception($"Serviço com ID {repairId} não encontrado.");
            }            

         
            repair.Mechanics.Clear();

       
            if (newMechanicIds != null && newMechanicIds.Any())
            {
                var newMechanics = await _context.Mechanics
                    .Where(m => newMechanicIds.Contains(m.Id))
                    .ToListAsync();

                foreach (var mechanic in newMechanics)
                {
                    repair.Mechanics.Add(mechanic);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveRepairAndServicesAsync(int id)
        {
            var service = await _context.RepairsAndServices
                .Include(r => r.Mechanics)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (service == null)
                return false;

           
            service.Mechanics.Clear();
            await _context.SaveChangesAsync(); 

            
            _context.RepairsAndServices.Remove(service);
            return await _context.SaveChangesAsync() > 0;
        }



    }
}
