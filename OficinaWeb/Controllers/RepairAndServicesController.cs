using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using OficinaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OficinaWeb.Controllers
{
    public class RepairAndServicesController : Controller
    {
        private readonly DataContext _context;
        private readonly IRepairAndServicesRepository _repairAndServicesRepository;
        private readonly IMechanicRepository _mechanicRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IVehicleRepository _vehicleRepository;

        public RepairAndServicesController(
            DataContext context,
            IRepairAndServicesRepository repairAndServicesRepository,
            IMechanicRepository mechanicRepository,
            IServiceTypeRepository serviceTypeRepository,
            IClientRepository clientRepository,
            IConverterHelper converterHelper,
            
            IVehicleRepository vehicleRepository
             )
        {
            _context = context;
            _repairAndServicesRepository = repairAndServicesRepository;
            _mechanicRepository = mechanicRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _clientRepository = clientRepository;
            _converterHelper = converterHelper;
            _vehicleRepository = vehicleRepository;
        }

        // GET: RepairAndServices
        public  IActionResult Index()
        {
            return View(_repairAndServicesRepository
                .GetAll()
                .Where(r => r.EndDate > DateTime.Now)
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .Include(r => r.Vehicle.CarBrand)
                .Include(r => r.Vehicle.CarModel)
                .Include(r => r.Mechanics)
                .Include(r => r.ServiceType)
                .OrderBy(r => r.BeginDate));
        }

        // GET: RepairAndServices/Details/5
        public async Task<IActionResult> Details(int? id, bool fromMyServices = false, int? vehicleId = null)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            var repairAndServices = await _repairAndServicesRepository.GetWithIncludesAsync(id.Value);
            if (repairAndServices == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            ViewData["FromMyServices"] = fromMyServices;
            ViewData["VehicleId"] = vehicleId;



            return View(repairAndServices);
        }

        // GET: RepairAndServices/Create
        public IActionResult Create()
        {
            var model = new RepairAndServicesViewModel();
            

            var serviceTypes = _serviceTypeRepository.GetAll().ToList();
            model.ServiceTypes = new SelectList(serviceTypes, "Id","Name");



            return View(model);
        }

        // POST: RepairAndServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairAndServicesViewModel model)
        {
            if (model.EndDate < DateTime.Today)
            {
                ModelState.AddModelError(nameof(model.EndDate), "End date cannot be in the past.");
            }

            if (model.EndDate <= model.BeginDate)
            {
                ModelState.AddModelError(nameof(model.EndDate), "End date must be after the begin date.");
            }

            if (model.BeginDate < DateTime.Today)
            {
                ModelState.AddModelError(nameof(model.BeginDate), "Begin date cannot be in the past.");
            }


            if (ModelState.IsValid)
            {
                var repairAndServices = _converterHelper.ToRepairAndServices(model, true);

                await _repairAndServicesRepository.CreateAsync(repairAndServices);

                await _repairAndServicesRepository.AddMechanics(repairAndServices.Id, repairAndServices.MechanicIds);
                

                return RedirectToAction("Index");
            }            

            var serviceTypes = _serviceTypeRepository.GetAll().ToList();
            model.ServiceTypes = new SelectList(serviceTypes, "Id", "Name");


            return View(model);
        }







        // GET: RepairAndServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            var repairAndServices = await _repairAndServicesRepository.GetWithIncludesAsync(id.Value);
            if (repairAndServices == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            var model = _converterHelper.ToRepairAndServicesViewModel(repairAndServices);

            var serviceTypes = _serviceTypeRepository.GetAll().ToList();
            model.ServiceTypes = new SelectList(serviceTypes, "Id", "Name");



            return View(model);
        }

        // POST: RepairAndServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RepairAndServices repairAndServices)
        {

           

            if (id != repairAndServices.Id)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            if (repairAndServices.EndDate < DateTime.Today)
            {
                ModelState.AddModelError(nameof(repairAndServices.EndDate), "End date cannot be in the past.");
            }

            if (repairAndServices.EndDate <= repairAndServices.BeginDate)
            {
                ModelState.AddModelError(nameof(repairAndServices.EndDate), "End date must be after the begin date.");
            }

            if (repairAndServices.BeginDate < DateTime.Today)
            {
                ModelState.AddModelError(nameof(repairAndServices.BeginDate), "Begin date cannot be in the past.");
            }


            if (ModelState.IsValid)
            {
                try
                {
                  await _repairAndServicesRepository.UpdateAsync(repairAndServices);
                    await _repairAndServicesRepository.UpdateMechanicsAsync(repairAndServices.Id, repairAndServices.MechanicIds);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _repairAndServicesRepository.ExistsAsync(repairAndServices.Id))
                    {
                        return new NotFoundViewResult("ServiceNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
       
            return View(repairAndServices);
        }

        // GET: RepairAndServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            var repairAndServices = await _repairAndServicesRepository.GetWithIncludesAsync(id.Value);
            if (repairAndServices == null)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }

            return View(repairAndServices);
        }

        // POST: RepairAndServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _repairAndServicesRepository.RemoveRepairAndServicesAsync(id);

            if (!result)
            {
                return new NotFoundViewResult("ServiceNotFound");
            }
                

            return RedirectToAction(nameof(Index));

        }



        [Authorize(Roles ="Employee,Client")]
        public async Task<IActionResult> MyServices(int? id)
        {
           var email = User.Identity.Name;


            if (this.User.IsInRole("Employee"))
            { 
                var mechanic = await  _mechanicRepository.GetByEmailAsync(email);

                if (mechanic == null)
                {
                   var services = await _repairAndServicesRepository.GetAll()
                    .Where(r => r.EndDate < DateTime.Now)
                    .Include(r => r.ServiceType)
                    .Include(r => r.Client)
                    .Include(r => r.Vehicle)
                     .ThenInclude(v => v.CarBrand)
                     .Include(r => r.Vehicle)
                     .ThenInclude(v => v.CarModel)
                    .Include(r => r.Parts)
                    .Include(r => r.Mechanics)
                    .ToListAsync();

                    if (services != null)
                    {
                        var model = new MyServicesViewModel
                        {
                            MyServices = services.ToList()
                        };

                        return View(model);
                    }
                }
                else
                {
                    var services = await _repairAndServicesRepository.GetAll()
                       .Where(r => r.Mechanics.Any(m => m.Id == mechanic.Id) && r.EndDate < DateTime.Now)
                       .Include(r => r.ServiceType)
                       .Include(r => r.Client)
                       .Include(r => r.Vehicle)
                        .ThenInclude(v => v.CarBrand)
                        .Include(r => r.Vehicle)
                        .ThenInclude(v => v.CarModel)
                       .Include(r => r.Parts)
                       .Include(r => r.Mechanics)
                       .ToListAsync();

                    if (services != null)
                    {
                        var model = new MyServicesViewModel
                        {
                            MyServices = services.ToList()
                        };

                        return View(model);
                    }
                }

                   
            }


            if (this.User.IsInRole("Client"))
            {

                var vehicleExists = await _vehicleRepository.ExistsAsync(id.Value);

                if (id == null || !vehicleExists)
                {
                    return NotFound();
                }

                var services = await VehicleInterventions(id.Value);

                //_repairAndServicesRepository.GetAll()
                //    .Where(r => r.VehicleId == id && r.EndDate < DateTime.Now)
                //    .Include(r => r.ServiceType)
                //    .Include(r => r.Client)
                //    .Include(r => r.Vehicle)
                //     .ThenInclude(v => v.CarBrand)
                //     .Include(r => r.Vehicle)
                //     .ThenInclude(v => v.CarModel)
                //    .Include(r => r.Parts)
                //    .Include(r => r.Mechanics)
                //    .ToListAsync();

                if (services != null)
                {
                    var model = new MyServicesViewModel
                    {
                        MyServices = services.Where(r => r.EndDate < DateTime.Now).ToList()
                    };

                    return View(model);
                }
            }



            return RedirectToAction("Index", "Home");

        }





        public async Task<List<RepairAndServices>> VehicleInterventions(int vehicleId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44311/");

                
                var response = await client.GetAsync($"api/VehicleInterventions?vehicleId={vehicleId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var interventions = JsonConvert.DeserializeObject<List<RepairAndServices>>(jsonString);

                    foreach (var item in interventions)
                    {
                        item.Client = await _clientRepository.GetIdAsync(item.ClientId);
                        item.ServiceType = await _serviceTypeRepository.GetIdAsync(item.ServiceTypeId);
                        item.Vehicle = await _vehicleRepository.GetByIdAsyncWithIncludes(item.VehicleId);
                        item.Mechanics = await _repairAndServicesRepository.GetMechanicsByServiceIdAsync(item.Id);
                        item.Parts = await _repairAndServicesRepository.GetPartsByServiceIdAsync(item.Id);
                    }

                    return (interventions);
                }

                return null;
                
            }
        }






        public IActionResult ServiceNotFound()
        {
            return View();
        }




    }


    
}
