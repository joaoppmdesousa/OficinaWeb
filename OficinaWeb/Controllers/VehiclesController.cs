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

        public VehiclesController(
            IVehicleRepository vehicleRepository,
            IUserHelper userHelper)
        {
            _vehicleRepository = vehicleRepository;
            _userHelper = userHelper;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index(int ? clientId)
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
        public async Task<IActionResult> Details(int? id)
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

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public async Task<IActionResult> Create(int? clientId)
        {
            if(clientId == null)
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
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Delete(int? id)
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
       
    }
}
