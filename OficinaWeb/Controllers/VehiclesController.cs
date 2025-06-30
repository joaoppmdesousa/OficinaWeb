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

        public VehiclesController(
            IVehicleRepository vehicleRepository,
            IUserHelper userHelper,
            IClientRepository clientRepository)
        {
            _vehicleRepository = vehicleRepository;
            _userHelper = userHelper;
            _clientRepository = clientRepository;
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
           .Where(v => v.ClientId == clientId.Value)
           .OrderBy(v => v.Model);

            return View(vehicles);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicleRepository.GetIdAsync(id.Value);
            if (vehicle == null)
            {
                return NotFound();
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

            ViewBag.ClientId = client.Id;

            var vehicle = new Vehicle
            {
                ClientId = clientId.Value
            };

            return View(vehicle);
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {

            if (ModelState.IsValid)
            {
                vehicle.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                //TODO: modificar para o user que tiver logado

                await _vehicleRepository.CreateAsync(vehicle);
                return RedirectToAction(nameof(Index), new { clientId = vehicle.ClientId });
            }

            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicleRepository.GetIdAsync(id.Value);
            if (vehicle == null)
            {
                return NotFound();
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Vehicles", new { clientId = vehicle.ClientId });

            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LicensePlate,Brand,Model,Year,Mileage,FuelType,ClientId")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vehicle.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    //TODO: modificar para o user que tiver logado

                    await _vehicleRepository.UpdateAsync(vehicle);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _vehicleRepository.ExistsAsync(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicleRepository.GetIdAsync(id.Value);
            if (vehicle == null)
            {
                return NotFound();
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


        public async Task<IActionResult> MyVehicles(string email)
        {
            email = User.Identity.Name;

            var client = await _clientRepository.GetByEmailAsync(email);

            if (client == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var vehicles = await _vehicleRepository.GetAll()
                .Where(v => v.ClientId == client.Id)
                .OrderBy(v => v.Model)
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
                .Where(v => v.ClientId == clientId)
                .Select(v => new
                {
                    id = v.Id,
                    name = v.Brand + " " + v.Model + " (" + v.LicensePlate + ")"
                })
                .ToList();

            return Json(vehicles);



        }

    }
}
