using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OficinaWeb.Data.Entities
{
    public class RepairAndServices : IEntity
    {
        public int Id { get ; set; }
      

        [Required]
        [Display(Name = "Type")]
        public string ServiceType { get; set; }

        public string Details { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }  

        public Client Client { get; set; }


        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }


        [Required]
        [Display(Name = "Begin Date")]
        public DateTime BeginDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal ServicePrice { get; set; }

 
        public List<Mechanic> Mechanics { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Select at least 1 mechanic.")]
        public List<int> MechanicIds { get; set; }

        public List<Part> Parts { get; set; }


        [NotMapped] 
        public decimal TotalPrice
        {
            get
            {
                var partsTotal = Parts?.Sum(p => p.UnitPrice) ?? 0;
                return ServicePrice + partsTotal;
            }
        }


        public string MechanicsNames => Mechanics != null
         ? string.Join(", ", Mechanics.Select(m => m.Name))
        : "N/A";


    }
}
