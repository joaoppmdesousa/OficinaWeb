using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using OficinaWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly DataContext _context;

        public AppointmentsController(
            IAppointmentsRepository appointmentsRepository,
            IClientRepository clientRepository,
            IConverterHelper converterHelper,
            DataContext context)
        {
            _appointmentsRepository = appointmentsRepository;
            _clientRepository = clientRepository;
            _converterHelper = converterHelper;
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
           
            return View(_appointmentsRepository.GetAll().Include(a => a.Client).Include(a => a.Mechanic).Include(a => a.Vehicle));
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentsRepository.GetByIdAsyncWithIncludes(id.Value);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            var model = new AppointmentViewModel
            {
                Clients = _clientRepository.GetAll().ToList(),
                Appointments = _appointmentsRepository.GetAll().ToList(),
                Date = DateTime.Now,
                AppointmentEnd = DateTime.Now.AddHours(1).TimeOfDay
            };
          

            ViewData["MechanicId"] = new SelectList(_context.Mechanics, "Id", "Name");
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Brand");
            return View(model);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = _converterHelper.ToAppointment(model, true);
                await _appointmentsRepository.CreateAsync(appointment);

                return RedirectToAction(nameof(Index));
            }
            
            ViewData["MechanicId"] = new SelectList(_context.Mechanics, "Id", "Name", model.MechanicId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Brand", model.VehicleId);

            model.Clients = _clientRepository.GetAll().ToList();
            model.Appointments = _appointmentsRepository.GetAll().ToList();


            return View(model);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentsRepository.GetByIdAsyncWithIncludes(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            var model =  _converterHelper.ToAppointmentViewModel(appointment);

            model.Clients = _clientRepository.GetAll().ToList();            
             
         
            ViewData["MechanicId"] = new SelectList(_context.Mechanics, "Id", "Name", appointment.MechanicId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Brand", appointment.VehicleId);

            return View(model);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,  AppointmentViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var appointment = _converterHelper.ToAppointment(model, false);

            if (ModelState.IsValid)
            {
                try
                {
                    

                   await _appointmentsRepository.UpdateAsync(appointment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _appointmentsRepository.ExistsAsync(model.Id))
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
           
            ViewData["MechanicId"] = new SelectList(_context.Mechanics, "Id", "Name", model.MechanicId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Brand", model.VehicleId);


            model.Clients = _clientRepository.GetAll().ToList();
            model.Client = await _clientRepository.GetIdAsync(appointment.ClientId);


            return View(model);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentsRepository.GetByIdAsyncWithIncludes(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                var appointment = await _appointmentsRepository.GetByIdAsyncWithIncludes(id.Value);
                await _appointmentsRepository.DeleteAsync(appointment);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(id);
            }
          
        }
          

    }
}
