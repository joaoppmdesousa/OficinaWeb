using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OficinaWeb.Data;

namespace OficinaWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            return Ok(_clientRepository.GetAll());
        }
    }
}
