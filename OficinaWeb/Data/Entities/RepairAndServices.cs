using System;
using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Data.Entities
{
    public class RepairAndServices : IEntity
    {
        public int Id { get ; set; }

        [Required]
        [Display(Name = "Begin Date")]
        public TimeSpan BeginDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public TimeSpan EndDate { get; set; }

        [Required]
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; }

        [Required]
        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

    }
}
