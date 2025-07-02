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

            if (serviceId == null)
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
                return NotFound();
            }

            ViewBag.ServiceId = serviceId;

            var part = await _context.Parts
                .Include(p => p.RepairAndServices)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // GET: Parts/Create
        public IActionResult Create(int? serviceId)
        {

            if (serviceId == null)
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
        public async Task<IActionResult> Create([Bind("Id,Name,Quantity,UnitPrice,RepairAndServicesId")] Part part)
        {
            if (ModelState.IsValid)
            {
                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { serviceId = part.RepairAndServicesId });
            }
           
            return View(part);
        }

        // GET: Parts/Edit/5
        public async Task<IActionResult> Edit(int? id, int serviceId)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.ServiceId = serviceId;

            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }
           
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quantity,UnitPrice,RepairAndServicesId")] Part part)
        {
            if (id != part.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartExists(part.Id))
                    {
                        return NotFound();
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
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.ServiceId = serviceId;

            var part = await _context.Parts
                .Include(p => p.RepairAndServices)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var part = await _context.Parts.FindAsync(id);
            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { serviceId = part.RepairAndServicesId });
        }

        private bool PartExists(int id)
        {
            return _context.Parts.Any(e => e.Id == id);
        }
    }
}
