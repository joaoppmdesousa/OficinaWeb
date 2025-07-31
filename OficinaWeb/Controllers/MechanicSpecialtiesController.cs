using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class MechanicSpecialtiesController : Controller
    {
        private readonly ISpecialtiesRepository _specialtiesRepository;
        private readonly IMechanicRepository _mechanicRepository;

        public MechanicSpecialtiesController(
            ISpecialtiesRepository specialtiesRepository,
            IMechanicRepository mechanicRepository)
        {
            _specialtiesRepository = specialtiesRepository;
            _mechanicRepository = mechanicRepository;
        }

        // GET: MechanicSpecialties
        public async Task<IActionResult> Index()
        {
            return View(_specialtiesRepository.GetAll());
        }

       

        // GET: MechanicSpecialties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MechanicSpecialties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] MechanicSpecialty mechanicSpecialty)
        {
            if (ModelState.IsValid)
            {
                await _specialtiesRepository.CreateAsync(mechanicSpecialty);
                return RedirectToAction(nameof(Index));
            }
            return View(mechanicSpecialty);
        }

        // GET: MechanicSpecialties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("SpecialitiesNotFound");
            }

            var mechanicSpecialty = await _specialtiesRepository.GetIdAsync(id.Value);
            if (mechanicSpecialty == null)
            {
                return new NotFoundViewResult("SpecialitiesNotFound");
            }
            return View(mechanicSpecialty);
        }

        // POST: MechanicSpecialties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] MechanicSpecialty mechanicSpecialty)
        {
            if (id != mechanicSpecialty.Id)
            {
                return new NotFoundViewResult("SpecialitiesNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _specialtiesRepository.UpdateAsync(mechanicSpecialty);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _specialtiesRepository.ExistsAsync(mechanicSpecialty.Id))
                    {
                        return new NotFoundViewResult("SpecialitiesNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mechanicSpecialty);
        }

        // GET: MechanicSpecialties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("SpecialitiesNotFound");
            }

            var mechanicSpecialty = await _specialtiesRepository.GetIdAsync(id.Value);
            if (mechanicSpecialty == null)
            {
                return new NotFoundViewResult("SpecialitiesNotFound");
            }

            return View(mechanicSpecialty);
        }

        // POST: MechanicSpecialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mechanicSpecialty = await _specialtiesRepository.GetIdAsync(id);


            var hasMechanic = _mechanicRepository.GetAll()
                         .Any(m => m.MechanicSpecialtyId == id);

            if (hasMechanic)
            {
                TempData["Error"] = "Cannot delete this specialty because they are assigned to 1 or more mechanics.";
                return RedirectToAction(nameof(Delete));
            }


            await _specialtiesRepository.DeleteAsync(mechanicSpecialty);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult SpecialitiesNotFound()
        {
            return View();
        }


    }
}
