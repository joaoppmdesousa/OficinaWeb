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
   
    public class MechanicsController : Controller
    {
        private readonly DataContext _context;
        private readonly IMechanicRepository _mechanicRepository;
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;

        public MechanicsController(
            DataContext context,
            IMechanicRepository mechanicRepository,
            IAppointmentsRepository appointmentsRepository,
            IConverterHelper converterHelper,
            IUserHelper userHelper)
        {
            _context = context;
            _mechanicRepository = mechanicRepository;
            _appointmentsRepository = appointmentsRepository;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
        }

        [Authorize(Roles = "Admin")]
        // GET: Mechanics
        public async Task<IActionResult> Index()
        {
            return View(_mechanicRepository.GetAll().Include(a => a.MechanicSpecialty));
        }


        // GET: Mechanics/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("MechanicNotFound");
            }

            var mechanic = await _mechanicRepository.GetByIdAsyncWithIncludes(id.Value);
            if (mechanic == null)
            {
                return new NotFoundViewResult("MechanicNotFound");
            }

            return View(mechanic);
        }


        // GET: Mechanics/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {

            ViewData["MechanicSpecialtyId"] = new SelectList(_context.Specialties, "Id", "Name");

            return View();
        }

        // POST: Mechanics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("MechanicNotFound");
            }

            var mechanic = await _mechanicRepository.GetIdAsync(id.Value);
            if (mechanic == null)
            {
                return new NotFoundViewResult("MechanicNotFound"); ;
            }

            ViewData["MechanicSpecialtyId"] = new SelectList(_context.Specialties, "Id", "Name", mechanic.MechanicSpecialtyId);

            return View(mechanic);
        }

        // POST: Mechanics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mechanic mechanic)
        {
            if (id != mechanic.Id)
            {
                return new NotFoundViewResult("MechanicNotFound");
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
                        return new NotFoundViewResult("MechanicNotFound");
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("MechanicNotFound");
            }

            var mechanic = await _mechanicRepository.GetIdAsync(id.Value);
            if (mechanic == null)
            {
                return new NotFoundViewResult("MechanicNotFound");
            }

            return View(mechanic);
        }

        // POST: Mechanics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mechanic = await _mechanicRepository.GetIdAsync(id);

            var hasAppointments = _appointmentsRepository.GetAll()
                          .Any(a => a.MechanicId == id);

            if (hasAppointments)
            {
                TempData["Error"] = "Cannot delete this mechanic because they are assigned to existing appointments.";
                return RedirectToAction(nameof(Delete));
            }

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



        public  IActionResult Schedule()
        {

            var appointmentsAux = _appointmentsRepository.GetAll()
                                                        .Include(a => a.Vehicle)
                                                            .ThenInclude(v => v.CarBrand)
                                                        .Include(a => a.Vehicle)
                                                            .ThenInclude(v => v.CarModel);

            var appointmentsViewModel = new List<ScheduleViewModel>();
            var mechanics =  _mechanicRepository.GetAll().Include(m => m.MechanicSpecialty);
            var isMechanic = mechanics.Any(m => m.Email == User.Identity.Name);
            var IsEmployee = User.IsInRole("Employee");
            var mechanic = mechanics.FirstOrDefault(m => m.Email == User.Identity.Name);

            foreach (Appointment appointment in appointmentsAux)
            {
                if(isMechanic)
                {
                    if(appointment.MechanicId == mechanic?.Id)
                    {
                        var viewModel = _converterHelper.ToScheduleViewModel(appointment, IsEmployee);
                        appointmentsViewModel.Add(viewModel);
                    }
                    
                }
                else
                {               
                    var viewModel = _converterHelper.ToScheduleViewModel(appointment, IsEmployee);
                    appointmentsViewModel.Add(viewModel);
                }

            }
           
            

            if (mechanics.Any())
            {
                
                var todayDate = DateTime.Today;

                if (isMechanic)
                {
                    ViewBag.StartHour = mechanic.ClockIn.ToString(@"hh\:mm");
                    ViewBag.EndHour = mechanic.ClockOut.ToString(@"hh\:mm");
                }
                else
                {
                    var earliestClockIn = mechanics.Min(m => m.ClockIn).ToString(@"hh\:mm");
                    var latestClockOut = mechanics.Max(m => m.ClockOut).ToString(@"hh\:mm");

                    ViewBag.StartHour = earliestClockIn;
                    ViewBag.EndHour = latestClockOut;
                }

                ViewBag.TodayDate = todayDate.ToString("yyyy-MM-dd");
            }



            ViewBag.Mechanics = mechanics;
            ViewBag.IsMechanic = isMechanic;


            return View(appointmentsViewModel);

        }


        public IActionResult MechanicNotFound()
        {
            return View();
        }



    }
}
