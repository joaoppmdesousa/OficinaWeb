using Microsoft.AspNetCore.Mvc;
using OficinaWeb.Data;

namespace OficinaWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MechanicsController : Controller
    {
        private readonly IMechanicRepository _mechanicRepository;

        public MechanicsController(IMechanicRepository mechanicRepository)
        {
            _mechanicRepository = mechanicRepository;
        }


        [HttpGet]
        public IActionResult GetVehicles()
        {
            return Ok(_mechanicRepository.GetAll());
        }
    }
}
