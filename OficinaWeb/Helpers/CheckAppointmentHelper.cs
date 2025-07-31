using OficinaWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaWeb.Helpers
{
    public class CheckAppointmentHelper : ICheckAppointmentHelper
    {
        private readonly IMechanicRepository _mechanicRepository;

        public CheckAppointmentHelper(IMechanicRepository mechanicRepository)
        {
            _mechanicRepository = mechanicRepository;
        }



        public async Task<bool> CheckScheduleConflictAsync(int mechanicId, DateTime StartTimeAndDate, TimeSpan EndTime, int? appointmentIdToExclude = null)
        {
            var mechanic = await _mechanicRepository.GetByIdAsyncWithIncludes(mechanicId);

            if (mechanic == null)
            {
                return true;
            }

            var newStart = StartTimeAndDate.TimeOfDay;
            var newEnd = EndTime;

            if (newStart < mechanic.ClockIn || newEnd > mechanic.ClockOut)
            {
                return true; 
            }

            var appointments = mechanic.Appointments;

            bool hasConflict = appointments.Any(a =>
               a.Id != appointmentIdToExclude &&                          
               a.Date.Date == StartTimeAndDate.Date &&
               newStart < a.AppointmentEnd &&
               a.Date.TimeOfDay < newEnd
                );



            return hasConflict;

        }    



    }
}
