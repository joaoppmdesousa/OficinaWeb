using System;
using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Data.Entities
{
    public class Appointment : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string AppointmentType { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Appointment End")]
        public TimeSpan AppointmentEnd { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        public Client Client { get; set; }

        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        [Required]
        [Display(Name = "Mechanic")]
        public int MechanicId { get; set; }


        public Mechanic Mechanic { get; set; }




    }
}
