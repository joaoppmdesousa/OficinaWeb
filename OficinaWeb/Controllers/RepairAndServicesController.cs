using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;

namespace OficinaWeb.Controllers
{
    public class RepairAndServicesController : Controller
    {
        private readonly DataContext _context;
        private readonly IRepairAndServicesRepository _repairAndServicesRepository;
        private readonly IMechanicRepository _mechanicRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public RepairAndServicesController(
            DataContext context,
            IRepairAndServicesRepository repairAndServicesRepository,
            IMechanicRepository mechanicRepository,
            IVehicleRepository vehicleRepository)
        {
            _context = context;
            _repairAndServicesRepository = repairAndServicesRepository;
            _mechanicRepository = mechanicRepository;
            _vehicleRepository = vehicleRepository;
        }

        // GET: RepairAndServices
        public  IActionResult Index()
        {
            return View(_repairAndServicesRepository
                .GetAll()
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .Include(r => r.Mechanics)
                .OrderBy(r => r.BeginDate));
        }

        // GET: RepairAndServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairAndServices = await _repairAndServicesRepository.GetWithIncludesAsync(id.Value);
            if (repairAndServices == null)
            {
                return NotFound();
            }

            return View(repairAndServices);
        }

        // GET: RepairAndServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RepairAndServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairAndServices repairAndServices)
        {

            if (ModelState.IsValid)
            {

                //var allMechanics = await _mechanicRepository.GetAll().ToListAsync();
                //repairAndServices.Mechanics = allMechanics
                //    .Where(m => repairAndServices.MechanicIds.Contains(m.Id))
                //    .ToList();

                await _repairAndServicesRepository.CreateAsync(repairAndServices);

                await _repairAndServicesRepository.AddMechanics(repairAndServices.Id, repairAndServices.MechanicIds);
                

                return RedirectToAction("Index");
            }
           
            return View(repairAndServices);
        }







        // GET: RepairAndServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairAndServices = await _repairAndServicesRepository.GetWithIncludesAsync(id.Value);
            if (repairAndServices == null)
            {
                return NotFound();
            }
           


            return View(repairAndServices);
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
                return NotFound();
            }

            // Log para depuração
            Console.WriteLine($"MechanicIds recebidos: {string.Join(", ", repairAndServices.MechanicIds ?? new List<int>())}");

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
                        return NotFound();
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
                return NotFound();
            }

            var repairAndServices = await _repairAndServicesRepository.GetIdAsync(id.Value);
            if (repairAndServices == null)
            {
                return NotFound();
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
                return NotFound();

            return RedirectToAction(nameof(Index));

        }


    }


    
}
