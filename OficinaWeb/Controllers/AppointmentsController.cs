using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using OficinaWeb.Models;
using Syncfusion.EJ2.Linq;
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
        private readonly IMechanicRepository _mechanicRepository;
        private readonly ICommunicationHelper _communicationHelper;
        private readonly IVehicleRepository _vehicleRepository;
       

        public AppointmentsController(
            IAppointmentsRepository appointmentsRepository,
            IClientRepository clientRepository,
            IConverterHelper converterHelper,
            IMechanicRepository mechanicRepository,
            ICommunicationHelper communicationHelper,
            IVehicleRepository vehicleRepository)
        {
            _appointmentsRepository = appointmentsRepository;
            _clientRepository = clientRepository;
            _converterHelper = converterHelper;
            _mechanicRepository = mechanicRepository;
            _communicationHelper = communicationHelper;
            _vehicleRepository = vehicleRepository;

        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            List<Appointment> appointments;

            if (this.User.IsInRole("Employee"))
            {
                var mechanics = _mechanicRepository.GetAll();
                var isMechanic = mechanics.Any(m => m.Email == User.Identity.Name);

                if (isMechanic)
                {
                    appointments = _appointmentsRepository.GetAll()
                                               .Include(a => a.Client)
                                               .Include(a => a.Mechanic)
                                               .Include(a => a.Vehicle)
                                                   .ThenInclude(v => v.CarBrand)
                                               .Include(a => a.Vehicle)
                                                   .ThenInclude(v => v.CarModel)
                                               .Where(a => a.Mechanic.Email == User.Identity.Name)
                                               .ToList();
                }
                else
                {
                    appointments = _appointmentsRepository.GetAll()
                                               .Include(a => a.Client)
                                               .Include(a => a.Mechanic)
                                               .Include(a => a.Vehicle)
                                                   .ThenInclude(v => v.CarBrand)
                                               .Include(a => a.Vehicle)
                                                   .ThenInclude(v => v.CarModel)
                                               .ToList();
                }
            }
            else
            {
                appointments = _appointmentsRepository.GetAll()
                                           .Include(a => a.Client)
                                           .Include(a => a.Mechanic)
                                           .Include(a => a.Vehicle)
                                               .ThenInclude(v => v.CarBrand)
                                           .Include(a => a.Vehicle)
                                               .ThenInclude(v => v.CarModel)
                                           .ToList();
            }

            return View(appointments);
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
            var clients = _clientRepository.GetAll()?.ToList() ?? new List<Client>();
            var appointments = _appointmentsRepository.GetAll()?.ToList() ?? new List<Appointment>();

            var model = new AppointmentViewModel
            {
                Clients = clients,
                Appointments = appointments,
                Date = DateTime.Now,
                AppointmentEnd = DateTime.Now.AddHours(1).TimeOfDay,
            };

            var mechanics =  _mechanicRepository.GetAll()?.ToList() ?? new List<Mechanic>();
            var vehicles = _vehicleRepository.GetAll()?.ToList() ?? new List<Vehicle>(); ;

            ViewBag.MechanicId = new SelectList(mechanics, "Id", "Name");
            ViewBag.VehicleId = new SelectList(vehicles, "Id", "CarBrand");

       

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

            var mechanics = await _mechanicRepository.GetAll().ToListAsync();
            var vehicles = await _vehicleRepository.GetAll().ToListAsync();

            ViewBag.MechanicId = new SelectList(mechanics, "Id", "Name");
            ViewBag.VehicleId = new SelectList(vehicles, "Id", "Brand");

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

            var mechanics = await _mechanicRepository.GetAll().ToListAsync();
            var vehicles = await _vehicleRepository.GetAll().ToListAsync();

            ViewData["MechanicId"] = new SelectList(mechanics, "Id", "Name", appointment.MechanicId);
            ViewData["VehicleId"] = new SelectList(vehicles, "Id", "Brand", appointment.VehicleId);

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

            var mechanics = await _mechanicRepository.GetAll().ToListAsync();
            var vehicles = await _vehicleRepository.GetAll().ToListAsync();

            ViewData["MechanicId"] = new SelectList(mechanics, "Id", "Name", model.MechanicId);
            ViewData["VehicleId"] = new SelectList(vehicles, "Id", "Brand", model.VehicleId);


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
                if (appointment == null)
                {
                    return NotFound();
                }
                var response = await _communicationHelper.CancelAppointmentNotificationAsync(appointment.Id);
                if (!response)
                {
                    TempData["Failed"] = "failed to send email";

                }

                await _appointmentsRepository.DeleteAsync(appointment);
                
            }
            catch (Exception)
            {
                return View(id);
            }

            return RedirectToAction(nameof(Index));

        }




        public async Task<IActionResult> AppointmentHistory()
        {
           var email = User.Identity.Name;

            if (this.User.IsInRole("Employee"))
            {
                var mechanic = await _mechanicRepository.GetByEmailAsync(email);

                if (mechanic == null)
                {
                    var appointments = _appointmentsRepository.GetAll()
                                                  .Where((a => a.Date < DateTime.Now))
                                                  .Include(a => a.Client)
                                                  .Include(a => a.Vehicle)
                                                    .ThenInclude(v => v.CarBrand)
                                                  .Include(a => a.Vehicle)
                                                    .ThenInclude(v => v.CarModel)
                                                  .Include(a => a.Mechanic);



                    if (appointments != null)
                    {
                        var model = new AppointmentHistoryViewModel
                        {
                            Appointments = appointments.ToList()
                        };

                        return View(model);
                    }

                }
                else
                {
                    var appointments = _appointmentsRepository.GetAll()
                                                 .Where(a => a.Mechanic.Id == mechanic.Id && a.Date < DateTime.Now)
                                                  .Include(a => a.Client)
                                                  .Include(a => a.Vehicle)
                                                    .ThenInclude(v => v.CarBrand)
                                                  .Include(a => a.Vehicle)
                                                    .ThenInclude(v => v.CarModel)
                                                  .Include(a => a.Mechanic);

                    if (appointments != null)
                    {
                        var model = new AppointmentHistoryViewModel
                        {
                            Appointments = appointments.ToList()
                        };

                        return View(model);
                    }
                }
            }


            if (this.User.IsInRole("Client"))
            {
                var client = await _clientRepository.GetByEmailAsync(email);
                if (client != null)
                {
                    var appointments = _appointmentsRepository.GetAll()
                                                 .Where(a => a.ClientId == client.Id && a.Date < DateTime.Now)
                                                  .Include(a => a.Client)
                                                  .Include(a => a.Vehicle)
                                                    .ThenInclude(v => v.CarBrand)
                                                  .Include(a => a.Vehicle)
                                                    .ThenInclude(v => v.CarModel)
                                                  .Include(a => a.Mechanic);

                    if (appointments != null)
                    {
                        var model = new AppointmentHistoryViewModel
                        {
                            Appointments = appointments.ToList()
                        };

                        return View(model);
                    }

                }
            }



                return NotFound();
        }
          



    }
}
