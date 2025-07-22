using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using OficinaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserHelper _userHelper;
        private readonly IClientRepository _clientRepository;
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly ICarModelRepository _carModelRepository;
        private readonly IConverterHelper _converterHelper;

        public VehiclesController(
            IVehicleRepository vehicleRepository,
            IUserHelper userHelper,
            IClientRepository clientRepository,
            ICarBrandRepository carBrandRepository,
            ICarModelRepository carModelRepository,
            IConverterHelper converterHelper)
        {
            _vehicleRepository = vehicleRepository;
            _userHelper = userHelper;
            _clientRepository = clientRepository;
            _carBrandRepository = carBrandRepository;
            _carModelRepository = carModelRepository;
            _converterHelper = converterHelper;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index(int? clientId)
        {
            if (clientId == null)
            {
                return NotFound();
            }

            var client = await _vehicleRepository.GetClientAsync(clientId.Value);
            if (client == null)
            {
                return NotFound();
            }

            ViewBag.ClientName = client.Name;
            ViewBag.ClientId = client.Id;
            var vehicles = _vehicleRepository
           .GetAll()
           .Include(v => v.CarBrand)
           .Include(v => v.CarModel)
           .Where(v => v.ClientId == clientId.Value)
           .OrderBy(v => v.CarModel);

           

            return View(vehicles);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            var vehicle = await _vehicleRepository.GetIdAsync(id.Value);
            if (vehicle == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Vehicles", new { clientId = vehicle.ClientId });

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public async Task<IActionResult> Create(int? clientId)
        {
            if (clientId == null)
            {
                return NotFound();
            }

            var client = await _vehicleRepository.GetClientAsync(clientId.Value);
            if (client == null)
            {
                return NotFound();
            }

            //ViewBag.ClientId = client.Id;

            var model = new VehicleViewModel
            {
                ClientId = clientId.Value,
                CarBrands = await _carBrandRepository.GetAll().ToListAsync()
            };

            ViewData["Brands"] = new SelectList(model.CarBrands, "Id", "Name");

            return View(model);
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel model)
        {

            if (ModelState.IsValid)
            {             
                var vehicle = _converterHelper.ToVehicle(model, true);
                await _vehicleRepository.CreateAsync(vehicle);
                return RedirectToAction(nameof(Index), new { clientId = vehicle.ClientId });
            }

            model.CarBrands = await _carBrandRepository.GetAll().ToListAsync();

            ViewData["Brands"] = new SelectList(model.CarBrands, "Id", "Name");


            return View(model);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            var vehicle = await _vehicleRepository.GetByIdAsyncWithIncludes(id.Value);
            if (vehicle == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Vehicles", new { clientId = vehicle.ClientId });

            var model = _converterHelper.ToVehicleViewModel(vehicle);

            model.CarBrands = await _carBrandRepository.GetAll().ToListAsync();

            ViewData["Brands"] = new SelectList(model.CarBrands, "Id", "Name", model.CarBrand);

            var models = await _carModelRepository.GetAll()
             .Where(m => m.CarBrandId == model.CarBrandId)
             .ToListAsync();

            ViewBag.Models = new SelectList(models, "Id", "Name", model.CarModel);

            return View(model);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleViewModel model)
        {
            if (id != model.Id)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            if (ModelState.IsValid)
            {
                var vehicle = _converterHelper.ToVehicle(model, false);

                try
                {              

                    await _vehicleRepository.UpdateAsync(vehicle);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _vehicleRepository.ExistsAsync(vehicle.Id))
                    {
                        return new NotFoundViewResult("VehicleNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { clientId = vehicle.ClientId });
            }

            model.CarBrands = await _carBrandRepository.GetAll().ToListAsync();

            var models = await _carModelRepository.GetAll()
            .Where(m => m.CarBrandId == model.CarBrandId)
             .ToListAsync();

            ViewBag.Models = new SelectList(models, "Id", "Name", model.CarModel);

            model.CarBrand = await _carBrandRepository.GetIdAsync(model.CarBrandId);

            return View(model);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            var vehicle = await _vehicleRepository.GetIdAsync(id.Value);
            if (vehicle == null)
            {
                return new NotFoundViewResult("VehicleNotFound");
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Vehicles", new { clientId = vehicle.ClientId });

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _vehicleRepository.GetIdAsync(id);
            await _vehicleRepository.DeleteAsync(vehicle);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> MyVehicles()
        {
            var email = User.Identity.Name;

            var client = await _clientRepository.GetByEmailAsync(email);

            if (client == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var vehicles = await _vehicleRepository.GetAll()
                .Include(v => v.CarBrand)
                .Include(v => v.CarModel)
                .Where(v => v.ClientId == client.Id)
                .OrderBy(v => v.CarModel)
                .ToListAsync();

            if (vehicles != null)
            {
                var model = new MyVehiclesViewModel
                {
                    MyVehicles = vehicles.ToList()
                };

                return View(model);
            }


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult GetVehiclesByClientId(int clientId)
        {
            var vehicles = _vehicleRepository.GetAll()
                 .Include(v => v.CarBrand)
                 .Include(v => v.CarModel)
                 .Where(v => v.ClientId == clientId)
                 .Select(v => new
                 {
                    id = v.Id,
                   name = v.CarBrand.Name + " " + v.CarModel.Name + " (" + v.LicensePlate + ")"
                 })
                 .ToList();

            return Json(vehicles);

        }



        [HttpGet]
        public IActionResult GetCarModelsByBrand(int brandId)
        {
            var carModels = _carModelRepository
                .GetAll()
                .Where(m => m.CarBrandId == brandId)
                .OrderBy(m => m.Name)
                .Select(m => new { m.Id, m.Name })
                .ToList();

            return Json(carModels);
        }



        public IActionResult VehicleNotFound()
        {
            return View();
        }

    }
}
