using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Controllers
{
    public class PartsController : Controller
    {
        private readonly DataContext _context;
        private readonly IPartsRepository _partsRepository;
        private readonly IRepairAndServicesRepository _repairAndServicesRepository;

        public PartsController(
            DataContext context,
            IPartsRepository partsRepository,
            IRepairAndServicesRepository repairAndServicesRepository)
        {
            _context = context;
            _partsRepository = partsRepository;
            _repairAndServicesRepository = repairAndServicesRepository;
        }

        // GET: Parts
        public async Task<IActionResult> Index(int? serviceId )
        {
            var serviceExists = await  _repairAndServicesRepository.ExistsAsync(serviceId.Value);

            if (serviceId == null || !serviceExists)
            {
                return NotFound();
            }

            
            ViewBag.ServiceId = serviceId;

            var parts = await _partsRepository
           .GetAll()
           .Where(p => p.RepairAndServicesId == serviceId.Value)
           .OrderBy(p => p.Name)
           .ToListAsync();


            return View(parts);                                             
        }

        // GET: Parts/Details/5
        public async Task<IActionResult> Details(int? id, int serviceId)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PartNotFound");
            }

            var serviceExists = await _repairAndServicesRepository.ExistsAsync(serviceId);

            if (!serviceExists)
            {
                return NotFound();
            }

            ViewBag.ServiceId = serviceId;

            var part = await _partsRepository.GetWithIncludesAsync(id.Value);
            if (part == null)
            {
                return new NotFoundViewResult("PartNotFound");
            }

            return View(part);
        }

        // GET: Parts/Create
        public async Task<IActionResult> Create(int? serviceId)
        {

            var serviceExists = await _repairAndServicesRepository.ExistsAsync(serviceId.Value);

            if (serviceId == null || !serviceExists)
            {
                return NotFound();
            }

            ViewBag.ServiceId = serviceId;
          
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Part part)
        {
            if (ModelState.IsValid)
            {
                await _partsRepository.CreateAsync(part);
                return RedirectToAction("Index", new { serviceId = part.RepairAndServicesId });
            }
           
            return View(part);
        }

        // GET: Parts/Edit/5
        public async Task<IActionResult> Edit(int? id, int serviceId)
        {
            var serviceExists = await _repairAndServicesRepository.ExistsAsync(serviceId);

            if (!serviceExists)
            {
                return NotFound();
            }

            if (id == null)
            {
                return new NotFoundViewResult("PartNotFound");
            }

            ViewBag.ServiceId = serviceId;

            var part = await _partsRepository.GetWithIncludesAsync(id.Value);
            if (part == null)
            {
                return new NotFoundViewResult("PartNotFound");
            }
           
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Part part)
        {
            if (id != part.Id)
            {
                return new NotFoundViewResult("PartNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _partsRepository.CreateAsync(part); 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _partsRepository.ExistsAsync(id))
                    {
                        return new NotFoundViewResult("PartNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { serviceId = part.RepairAndServicesId });
            }
           
            return View(part);
        }

        // GET: Parts/Delete/5
        public async Task<IActionResult> Delete(int? id, int serviceId)
        {
            var serviceExists = await _repairAndServicesRepository.ExistsAsync(serviceId);

            if (!serviceExists)
            {
                return NotFound();
            }

            if (id == null)
            {
                return new NotFoundViewResult("PartNotFound");
            }

            ViewBag.ServiceId = serviceId;

            var part = await _partsRepository.GetWithIncludesAsync(id.Value);
            if (part == null)
            {
                return new NotFoundViewResult("PartNotFound");
            }

            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var part = await _partsRepository.GetWithIncludesAsync(id);
            await _partsRepository.DeleteAsync(part);
            return RedirectToAction("Index", new { serviceId = part.RepairAndServicesId });
        }

       


        public IActionResult PartNotFound()
        {
            return View();
        }
    }
}
