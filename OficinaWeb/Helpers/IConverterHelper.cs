using OficinaWeb.Data.Entities;
using OficinaWeb.Models;
using System;

namespace OficinaWeb.Helpers
{
    public interface IConverterHelper
    {

        Appointment ToAppointment(AppointmentViewModel model, bool isNew);

        AppointmentViewModel ToAppointmentViewModel(Appointment appointment);
    }
}
