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
using OficinaWeb.Models;

namespace OficinaWeb.Controllers
{
    
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserHelper _userHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly DataContext _context;

        public ClientsController(
            IClientRepository clientRepository,
            IUserHelper userHelper,
            IEmailHelper emailHelper,
            DataContext context)
        {
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            _emailHelper = emailHelper;
            _context = context;
        }

        [Authorize(Roles = "Employee")]
        // GET: Clients
        public IActionResult Index()
        {
            return View(_clientRepository.GetAll().OrderBy(c => c.Name));
        }


        // GET: Clients Users
        [Authorize(Roles = "Admin")]
        public IActionResult AdminClientList()
        {
            var allClients = _clientRepository.GetAll().ToList();
            var allUsers = _userHelper.GetAllUsers();

            var userEmails = allUsers.Select(u => u.UserName.ToLower()).ToHashSet();

            var clientsWithUser = allClients
                .Where(c => userEmails.Contains(c.Email.ToLower()))
                .OrderBy(c => c.Name)
                .ToList();

            var clientsWithoutUser = allClients
                .Where(c => !userEmails.Contains(c.Email.ToLower()))
                .OrderBy(c => c.Name)
                .ToList();

            var model = new AdminClientListViewModel
            {
                ClientsWithUser = clientsWithUser,
                ClientsWithoutUser = clientsWithoutUser
            };

            return View(model);


        }

        [Authorize(Roles = "Employee")]
        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            var client = await _clientRepository.GetIdAsync(id.Value);
            if (client == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            return View(client);
        }


        [Authorize(Roles = "Employee")]
        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Employee")]
        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Client client)
        {
            if (ModelState.IsValid)
            {

                if (_emailHelper.CheckEmailExists(client.Email, 0, true))
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                }

                await _clientRepository.CreateAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        [Authorize(Roles = "Employee")]
        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            var client = await _clientRepository.GetIdAsync(id.Value);
            if (client == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }
            return View(client);
        }


        [Authorize(Roles = "Employee")]
        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Contact,TaxNumber,Email")] Client client)
        {
            if (id != client.Id)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            if (ModelState.IsValid)
            {

                if (_emailHelper.CheckEmailExists(client.Email, client.Id, true))
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                }

                try
                {
                    await _clientRepository.UpdateAsync(client);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _clientRepository.ExistsAsync(client.Id))
                    {
                        return new NotFoundViewResult("ClientNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        [Authorize(Roles = "Employee")]
        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            var client = await _clientRepository.GetIdAsync(id.Value);
            if (client == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            return View(client);
        }

        [Authorize(Roles = "Employee")]
        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _clientRepository.GetIdAsync(id);
            await _clientRepository.DeleteAsync(client);
            return RedirectToAction(nameof(Index));
        }




        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult SearchClients(string search)
        {
            var clients = _clientRepository.GetAll()
                .Where(c => string.IsNullOrEmpty(search) || c.Name.Contains(search))
                .OrderBy(c => c.Name)
                .Select(c => new {
                    id = c.Id,
                    name = c.Name,
                    email = c.Email
                })
                .ToList();

            return Json(clients);
        }

        public IActionResult ClientNotFound()
        {
            return View();
        }



    }
}
