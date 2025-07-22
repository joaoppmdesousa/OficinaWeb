using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
using OficinaWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepairAndServicesRepository _repairAndServicesRepository;
        private readonly IMechanicRepository _mechanicRepository;
        private readonly IAppointmentsRepository _appointmentsRepository;

        public HomeController(ILogger<HomeController> logger, IRepairAndServicesRepository repairAndServicesRepository, IMechanicRepository mechanicRepository, IAppointmentsRepository appointmentsRepository)
        {
            _logger = logger;
            _repairAndServicesRepository = repairAndServicesRepository;
            _mechanicRepository = mechanicRepository;
            _appointmentsRepository = appointmentsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Dashboard()
        {
            var mechanics = _mechanicRepository.GetAll();
            var services = new List<RepairAndServices>();
            var appointments = new List<Appointment>();
            bool isMechanic = false;

            if(mechanics != null)
            {
                if(mechanics.Any(m => m.Email == this.User.Identity.Name))
                {
                   services = _repairAndServicesRepository.GetAll()
                                                       .Where(s => s.EndDate > DateTime.Today &&
                                                                s.Mechanics.Any(m => m.Email == this.User.Identity.Name))
                                                       .Include(s => s.Client)
                                                       .Include(s => s.Mechanics)
                                                       .Include(s => s.Vehicle)
                                                           .ThenInclude(v => v.CarBrand)
                                                       .Include(s => s.Vehicle)
                                                           .ThenInclude(v => v.CarModel)
                                                        .Include(s => s.ServiceType)
                                                           .ToList();

                    appointments = _appointmentsRepository.GetAll()
                                                     .Where(a => a.Date.Date == DateTime.Today && a.Mechanic.Email == this.User.Identity.Name)
                                                     .Include(a => a.Client)
                                                     .Include(a => a.Mechanic)
                                                     .Include(a => a.Vehicle)
                                                        .ThenInclude(v => v.CarBrand)
                                                     .Include(a => a.Vehicle)
                                                        .ThenInclude(v => v.CarModel)
                                                     .ToList();
                    isMechanic = true;

                }
                else
                {
                    services = _repairAndServicesRepository.GetAll()
                                                          .Where(s => s.EndDate > DateTime.Today)
                                                          .Include(s => s.Client)
                                                          .Include (s => s.Mechanics)
                                                          .Include(s => s.Vehicle)
                                                              .ThenInclude(v => v.CarBrand)
                                                          .Include(s => s.Vehicle)
                                                              .ThenInclude(v => v.CarModel)
                                                          .Include(s => s.ServiceType)
                                                              .ToList();

                    appointments = _appointmentsRepository.GetAll()
                                                     .Where(a => a.Date.Date == DateTime.Today)
                                                     .Include(a => a.Client)
                                                     .Include(a => a.Mechanic)
                                                     .Include(a => a.Vehicle)
                                                        .ThenInclude(v => v.CarBrand)
                                                     .Include(a => a.Vehicle)
                                                        .ThenInclude(v => v.CarModel)
                                                     .ToList();
                }
            }
           



            var model = new DashboardViewModel
            {
                cellSpacing = new double[] { 10, 10 },
                Services = services,
                Appointments = appointments,
                IsMechanic = isMechanic
            };



            return View(model);
        }



    }
}
