using Microsoft.AspNetCore.Mvc.Rendering;
using OficinaWeb.Data.Entities;
using System.Collections;
using System.Collections.Generic;

namespace OficinaWeb.Models
{
    public class AppointmentViewModel : Appointment
    {

        public List<Client> Clients { get; set; }

        public IEnumerable<SelectListItem> MechanicsList { get; set; }
        public IEnumerable<SelectListItem> VehiclesList { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}
