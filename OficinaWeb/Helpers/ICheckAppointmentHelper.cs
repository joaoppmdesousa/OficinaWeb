using System;
using System.Threading.Tasks;

namespace OficinaWeb.Helpers
{
    public interface ICheckAppointmentHelper
    {
        Task<bool> CheckScheduleConflictAsync(int mechanicId, DateTime StartTimeAndDate, TimeSpan EndTime, int? appointmentIdToExclude = null);
    }
}
