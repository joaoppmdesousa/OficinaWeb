using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using OficinaWeb.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Controllers
{
    public class CommunicationController : Controller
    {
        
        private readonly ICommunicationHelper _communicationHelper;
        private readonly IEmailHelper _emailHelper;

        public CommunicationController(
            ICommunicationHelper communicationHelper,
            IEmailHelper emailHelper)
        {            
            _communicationHelper = communicationHelper;
            _emailHelper = emailHelper;
        }
        public async Task<IActionResult> Index()
        {
            var model = new CommunicationPanelViewModel();

            return View(model);
        }


        public async Task<IActionResult> NotifyAllClients(NotifyViewModel model)
        {

            if (model.BeginDate < DateTime.Now)
            {
                ModelState.AddModelError(nameof(model.BeginDate), "Begin date cannot be in the past.");
            }

            if (model.EndDate < model.BeginDate)
            {
                ModelState.AddModelError(nameof(model.EndDate), "End date must be after the begin date.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", new CommunicationPanelViewModel
                {
                    NotifyFormModel = model
                });
            }
            


            var success = await _communicationHelper.NotifyAllClientsAsync(model.BeginDate, model.EndDate);

            TempData["NotifyMessage"] = success
                ? "Emails sent successfully to all clients."
                : "Failed to send emails to one or more clients.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReminderAppointments()
        {
            var success = await _communicationHelper.SendAppointmentRemindersAsync();

            TempData["ReminderMessage"] = success
                ? "Reminders sent successfully."
                : "There are no appointments in the next 24 hours or some reminders failed to send.";

            return RedirectToAction("Index");
        }


        public IActionResult CustomMessage(CustomMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new CommunicationPanelViewModel
                {
                    CustomFormModel = model
                });
            }

            var email = model.Email;
            var subject = model.Subject;
            var body = model.Body;

            var response = _emailHelper.SendEmail(email, subject, body);
            var success = response.IsSuccess;

            if (success)
            {
                TempData["CustomMessage"] = success
                   ? "Email sent successfully."
                   : "Error sending email.";
            }

            return RedirectToAction("Index");
        }
 

    }
}
