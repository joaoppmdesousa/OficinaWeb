﻿using Microsoft.AspNetCore.Mvc;
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
    public class ServiceTypesController : Controller
    {

        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IRepairAndServicesRepository _repairAndServicesRepository;

        public ServiceTypesController(
            IServiceTypeRepository serviceTypeRepository,
            IRepairAndServicesRepository repairAndServicesRepository)
        {

            _serviceTypeRepository = serviceTypeRepository;
            _repairAndServicesRepository = repairAndServicesRepository;
        }

        // GET: ServiceTypes
        public async Task<IActionResult> Index()
        {
            return View(await _serviceTypeRepository.GetAll().ToListAsync());
        }


        // GET: ServiceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceType serviceType)
        {
            if (ModelState.IsValid)
            {
                await _serviceTypeRepository.CreateAsync(serviceType);
                return RedirectToAction(nameof(Index));
            }
            return View(serviceType);
        }

        // GET: ServiceTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }

            var serviceType = await _serviceTypeRepository.GetIdAsync(id.Value);
            if (serviceType == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }
            return View(serviceType);
        }

        // POST: ServiceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceType serviceType)
        {
            if (id != serviceType.Id)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceTypeRepository.UpdateAsync(serviceType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _serviceTypeRepository.ExistsAsync(id))
                    {
                        return new NotFoundViewResult("ServiceTypeNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(serviceType);
        }

        // GET: ServiceTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }

            var serviceType = await _serviceTypeRepository.GetIdAsync(id.Value);
            if (serviceType == null)
            {
                return new NotFoundViewResult("ServiceTypeNotFound");
            }

            return View(serviceType);
        }

        // POST: ServiceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceType = await _serviceTypeRepository.GetIdAsync(id);

            var hasService = _repairAndServicesRepository.GetAll()
                         .Any(s => s.ServiceTypeId == id);

            if (hasService)
            {
                TempData["Error"] = "Cannot delete this service type because they are assigned to existing services.";
                return RedirectToAction(nameof(Delete));
            }


            await _serviceTypeRepository.DeleteAsync(serviceType);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult ServiceTypeNotFound()
        {
            return View();
        }


    }
}
