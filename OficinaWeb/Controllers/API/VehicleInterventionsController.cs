using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data;
using Syncfusion.EJ2.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleInterventionsController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRepairAndServicesRepository _repairAndServicesRepository;

        public VehicleInterventionsController(IVehicleRepository vehicleRepository, IRepairAndServicesRepository repairAndServicesRepository)
        {
            _vehicleRepository = vehicleRepository;
            _repairAndServicesRepository = repairAndServicesRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetVehicleIntervention(int vehicleId)
        {
            if(await _vehicleRepository.ExistsAsync(vehicleId))
            {

                var interventions = _repairAndServicesRepository.GetAll()
                                        .Where(r => r.VehicleId == vehicleId)
                                        .Include(r => r.Client)
                                        .Include(r => r.ServiceType)
                                        .Include(r => r.Mechanics)
                                        .Include(r => r.Parts)
                                        .Include(r => r.Vehicle)
                                         .ThenInclude(v => v.CarBrand)
                                        .Include(r => r.Vehicle)
                                         .ThenInclude(v => v.CarModel)
                                        .ToList();
                return Ok(interventions);
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
