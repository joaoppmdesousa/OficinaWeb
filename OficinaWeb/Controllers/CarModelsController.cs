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
    public class CarModelsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICarModelRepository _carModelRepository;
        private readonly ICarBrandRepository _carBrandRepository;

        public CarModelsController(DataContext context, ICarModelRepository carModelRepository, ICarBrandRepository carBrandRepository)
        {
            _context = context;
            _carModelRepository = carModelRepository;
            _carBrandRepository = carBrandRepository;
        }

        // GET: CarModels
        public async Task<IActionResult> Index(int? brandId)
        {
             

            var models = _carModelRepository.GetAll().Include(c => c.CarBrand).Where(c => c.CarBrandId == brandId.Value);

            var brand = _carBrandRepository.GetIdAsync(brandId.Value);

            ViewBag.CarBrand = brand.Result.Name;
            ViewBag.CarBrandId = brandId;

            return View(models);
        }


        // GET: CarModels/Create
        public IActionResult Create(int? brandId)
        {
            if (brandId == null)
            {
                return NotFound();
            }

            ViewBag.CarBrandId = brandId;

            return View();
        }

        // POST: CarModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CarBrandId")] CarModel carModel)
        {
            if (ModelState.IsValid)
            {
                await _carModelRepository.CreateAsync(carModel);
                return RedirectToAction("Index", new { brandId = carModel.CarBrandId });
            }
           
            return View(carModel);
        }

        // GET: CarModels/Edit/5
        public async Task<IActionResult> Edit(int? id, int brandId)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CarBrandId = brandId;

            var carModel = await _carModelRepository.GetIdAsync(id.Value);
            if (carModel == null)
            {
                return NotFound();
            }

            return View(carModel);
        }

        // POST: CarModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarModel carModel)
        {
            if (id != carModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _carModelRepository.UpdateAsync(carModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _carModelRepository.ExistsAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { brandId = carModel.CarBrandId });
            }

            return View(carModel);
        }

        // GET: CarModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _carModelRepository.GetIdAsync(id.Value);
            if (carModel == null)
            {
                return NotFound();
            }

            return View(carModel);
        }

        // POST: CarModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carModel = await _carModelRepository.GetIdAsync(id);
            await _carModelRepository.DeleteAsync(carModel);

            return RedirectToAction("Index", new { brandId = carModel.CarBrandId });
        }

    }
}
