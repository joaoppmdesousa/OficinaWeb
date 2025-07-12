using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;

namespace OficinaWeb.Controllers
{
    [Authorize(Roles ="Admin")]
    public class MechanicsController : Controller
    {
        private readonly DataContext _context;
        private readonly IMechanicRepository _mechanicRepository;
        private readonly IUserHelper _userHelper;

        public MechanicsController(
            DataContext context,
            IMechanicRepository mechanicRepository,
            IUserHelper userHelper)
        {
            _context = context;
            _mechanicRepository = mechanicRepository;
            _userHelper = userHelper;
        }

        // GET: Mechanics
        public async Task<IActionResult> Index()
        {
            return View(_mechanicRepository.GetAll().Include(a => a.MechanicSpecialty));
        }

        // GET: Mechanics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _mechanicRepository.GetByIdAsyncWithIncludes(id.Value);
            if (mechanic == null)
            {
                return NotFound();
            }

            return View(mechanic);
        }

        // GET: Mechanics/Create
        public IActionResult Create()
        {

            ViewData["MechanicSpecialtyId"] = new SelectList(_context.Specialties, "Id", "Name");

            return View();
        }

        // POST: Mechanics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Mechanic mechanic)
        {
            if (ModelState.IsValid)
            {
                await _mechanicRepository.CreateAsync(mechanic);
                return RedirectToAction(nameof(Index));
            }

            ViewData["MechanicSpecialtyId"] = new SelectList(_context.Specialties, "Id", "Name");

            return View(mechanic);
        }

        // GET: Mechanics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _mechanicRepository.GetIdAsync(id.Value);
            if (mechanic == null)
            {
                return NotFound();
            }

            ViewData["MechanicSpecialtyId"] = new SelectList(_context.Specialties, "Id", "Name", mechanic.MechanicSpecialtyId);

            return View(mechanic);
        }

        // POST: Mechanics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mechanic mechanic)
        {
            if (id != mechanic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _mechanicRepository.UpdateAsync(mechanic);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _mechanicRepository.ExistsAsync(id))
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

            ViewData["MechanicSpecialtyId"] = new SelectList(_context.Specialties, "Id", "Name", mechanic.MechanicSpecialtyId);

            return View(mechanic);
        }

        // GET: Mechanics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _mechanicRepository.GetIdAsync(id.Value);
            if (mechanic == null)
            {
                return NotFound();
            }

            return View(mechanic);
        }

        // POST: Mechanics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mechanic = await _mechanicRepository.GetIdAsync(id);
            await _mechanicRepository.DeleteAsync(mechanic);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> SearchMechanics(string search)
        {
            var mechanics = await _mechanicRepository.GetAll()
                .Where(m => string.IsNullOrEmpty(search) || m.Name.Contains(search))
                .Select(m => new
                {
                    id = m.Id,
                    name = m.Name,
                })
                .ToListAsync();

            return Json(mechanics);
        }


    }
}
