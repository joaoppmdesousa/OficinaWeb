using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OficinaWeb.Data;

namespace OficinaWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehiclesController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        [HttpGet]
        public IActionResult GetVehicles()
        {
            return Ok(_vehicleRepository.GetAll());
        }
    }
}
