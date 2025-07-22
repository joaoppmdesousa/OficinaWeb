using OficinaWeb.Data.Entities;
using System.Collections;
using System.Collections.Generic;

namespace OficinaWeb.Models
{
    public class DashboardViewModel
    {
        public double[] cellSpacing { get; set; }

        public List<RepairAndServices> Services { get; set; }

        public List<Appointment> Appointments { get; set; }

        public bool IsMechanic { get; set; }
    }
}
