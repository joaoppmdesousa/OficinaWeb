using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Helpers
{
    public class CommunicationHelper : ICommunicationHelper
    {
        private readonly IEmailHelper _emailHelper;
        private readonly IClientRepository _clientRepository;
        private readonly IAppointmentsRepository _appointmentsRepository;

        public CommunicationHelper(IEmailHelper emailHelper,
        IClientRepository clientRepository,
        IAppointmentsRepository appointmentsRepository)
        {
            _emailHelper = emailHelper;
            _clientRepository = clientRepository;
            _appointmentsRepository = appointmentsRepository;
        }


        public async Task<bool> NotifyAllClientsAsync(DateTime begin, DateTime end)
        {
            var subject = "Workshop Warning";
            var body = $@"
                    <div style='font-family: Arial, sans-serif; color: #333; font-size: 16px; line-height: 1.6;'>
                        <p>Dear client,</p>
                        <p>We would like to inform you that the workshop will be <strong>closed from {begin:dd/MM/yyyy}</strong> to <strong>{end:dd/MM/yyyy}</strong> due to unforeseen circumstances.</p>
                        <p>We appreciate your understanding and apologize for any inconvenience this may cause.</p>
                        <p>Kind regards,<br><strong>Your Workshop Team</strong></p>
                    </div>";

            var clients = await _clientRepository.GetAll().ToListAsync();
            bool anyFailed = false;

            foreach (var client in clients)
            {
                try
                {
                    var response = _emailHelper.SendEmail(client.Email, subject, body);
                    if (!response.IsSuccess)
                        anyFailed = true;
                }
                catch
                {
                    anyFailed = true;
                }
            }

            return !anyFailed;
        }

        public async Task<bool> SendAppointmentRemindersAsync()
        {
            var tomorrow = DateTime.Today.AddDays(1);
            var appointments = await _appointmentsRepository.GetAll()
                .Where(a => a.Date.Date == tomorrow)
                .Include(a => a.Client)
                .ToListAsync();


            if (!appointments.Any())
            {               
                return false;
            }

            bool anyFailed = false;

            foreach (var appointment in appointments)
            {
                var subject = "Appointment Reminder for Tomorrow";
                var body = $"<p>Hello {appointment.Client.Name},</p>" +
                           $"<p>This is a reminder that you have an appointment scheduled for <strong>{appointment.Date:dd/MM/yyyy HH:mm}</strong>.</p>" +
                           $"<p>See you soon!</p>";

                try
                {
                    var response = _emailHelper.SendEmail(appointment.Client.Email, subject, body);
                    if (!response.IsSuccess)
                    {
                        anyFailed = true; 
                    }
                }
                catch
                {
                    anyFailed = true;
                }
            }

            return !anyFailed;
        }

        public async Task<bool> CancelAppointmentNotificationAsync(int appointmentId)
        {
            var appointment = await _appointmentsRepository.GetAll()
                .Include(a => a.Client)
                .Include(a => a.Mechanic)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                return false;

            var subject = "Appointment Cancellation";
            var body = $"<p>Dear {appointment.Client.Name},</p>" +
                       $"<p>We regret to inform you that your appointment scheduled for <strong>{appointment.Date:dd/MM/yyyy HH:mm}</strong> with mechanic <strong>{appointment.Mechanic.Name}</strong> has been cancelled.</p>" +
                       $"<p>We apologize for the inconvenience. Please contact us to reschedule.</p>";

            try
            {
                var response = _emailHelper.SendEmail(appointment.Client.Email, subject, body);
                return response.IsSuccess;
            }
            catch
            {
                return false;
            }
        }
    }
}