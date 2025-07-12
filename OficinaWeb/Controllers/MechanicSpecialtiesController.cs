using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
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

        public MechanicSpecialtiesController(
            ISpecialtiesRepository specialtiesRepository)
        {
            _specialtiesRepository = specialtiesRepository;
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
                return NotFound();
            }

            var mechanicSpecialty = await _specialtiesRepository.GetIdAsync(id.Value);
            if (mechanicSpecialty == null)
            {
                return NotFound();
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
                return NotFound();
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
                        return NotFound();
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
                return NotFound();
            }

            var mechanicSpecialty = await _specialtiesRepository.GetIdAsync(id.Value);
            if (mechanicSpecialty == null)
            {
                return NotFound();
            }

            return View(mechanicSpecialty);
        }

        // POST: MechanicSpecialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mechanicSpecialty = await _specialtiesRepository.GetIdAsync(id);
            await _specialtiesRepository.DeleteAsync(mechanicSpecialty);
            return RedirectToAction(nameof(Index));
        }

    }
}
