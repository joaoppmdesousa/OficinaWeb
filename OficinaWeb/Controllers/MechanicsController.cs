﻿using System;
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
    [Authorize]
    public class MechanicsController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public MechanicsController(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        // GET: Mechanics
        public async Task<IActionResult> Index()
        {
            return View(await _context.Mechanics.ToListAsync());
        }

        // GET: Mechanics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _context.Mechanics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mechanic == null)
            {
                return NotFound();
            }

            return View(mechanic);
        }

        // GET: Mechanics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mechanics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Specialty,ProfessionalContact,ClockIn,ClockOut,Active")] Mechanic mechanic)
        {
            if (ModelState.IsValid)
            {
                mechanic.User = await _userHelper.GetUserByEmailAsync("joaopedropsousa@gmail.com");
                //TODO: modificar para o user que tiver logado

                if (mechanic.ClockOut <= mechanic.ClockIn)
                {
                    ModelState.AddModelError("ClockOut", "Clock Out must be after Clock In.");
                    return View(mechanic);
                }

                _context.Add(mechanic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mechanic);
        }

        // GET: Mechanics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _context.Mechanics.FindAsync(id);
            if (mechanic == null)
            {
                return NotFound();
            }
            return View(mechanic);
        }

        // POST: Mechanics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Specialty,ProfessionalContact,ClockIn,ClockOut,Active")] Mechanic mechanic)
        {
            if (id != mechanic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    mechanic.User = await _userHelper.GetUserByEmailAsync("joaopedropsousa@gmail.com");
                    //TODO: modificar para o user que tiver logado

                    if (mechanic.ClockOut <= mechanic.ClockIn)
                    {
                        ModelState.AddModelError("ClockOut", "Clock Out must be after Clock In.");
                        return View(mechanic);
                    }

                    _context.Update(mechanic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MechanicExists(mechanic.Id))
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
            return View(mechanic);
        }

        // GET: Mechanics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _context.Mechanics
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var mechanic = await _context.Mechanics.FindAsync(id);
            _context.Mechanics.Remove(mechanic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MechanicExists(int id)
        {
            return _context.Mechanics.Any(e => e.Id == id);
        }
    }
}
