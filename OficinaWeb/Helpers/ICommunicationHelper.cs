using System;
using System.Threading.Tasks;

namespace OficinaWeb.Helpers
{
    public interface ICommunicationHelper
    {

        Task<bool> NotifyAllClientsAsync(DateTime begin, DateTime end);
        Task<bool> SendAppointmentRemindersAsync();
        Task<bool> CancelAppointmentNotificationAsync(int appointmentId);


    }
}
