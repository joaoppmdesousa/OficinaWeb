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
    public class CarBrandsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICarBrandRepository _carBrandRepository;

        public CarBrandsController(DataContext context, ICarBrandRepository carBrandRepository)
        {
            _context = context;
            _carBrandRepository = carBrandRepository;
        }

        // GET: CarBrands
        public async Task<IActionResult> Index()
        {
            return View( _carBrandRepository.GetAll());
        }

 

        // GET: CarBrands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarBrands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarBrand carBrand)
        {
            if (ModelState.IsValid)
            {
                await _carBrandRepository.CreateAsync(carBrand);
                return RedirectToAction(nameof(Index));
            }
            return View(carBrand);
        }

        // GET: CarBrands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CarBrandNotFound");
            }

            var carBrand = await _carBrandRepository.GetIdAsync(id.Value);
            if (carBrand == null)
            {
                return new NotFoundViewResult("CarBrandNotFound");
            }
            return View(carBrand);
        }

        // POST: CarBrands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarBrand carBrand)
        {
            if (id != carBrand.Id)
            {
                return new NotFoundViewResult("CarBrandNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _carBrandRepository.UpdateAsync(carBrand);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _carBrandRepository.ExistsAsync(id))
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
            return View(carBrand);
        }

        // GET: CarBrands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CarBrandNotFound");
            }

            var carBrand = await _carBrandRepository.GetIdAsync(id.Value);
            if (carBrand == null)
            {
                return new NotFoundViewResult("CarBrandNotFound");
            }

            return View(carBrand);
        }

        // POST: CarBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carBrand = await _carBrandRepository.GetIdAsync(id);
            await _carBrandRepository.DeleteAsync(carBrand);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult CarBrandNotFound()
        {
            return View();
        }



    }
}
