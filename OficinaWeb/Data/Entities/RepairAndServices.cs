using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace OficinaWeb.Data.Entities
{
    public class RepairAndServices : IEntity
    {
        public int Id { get ; set; }
      

        [Required]
        [Display(Name = "Type")]
        public int ServiceTypeId { get; set; }


        [JsonIgnore]
        public ServiceType ServiceType { get; set; }

        public string Details { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }


        [JsonIgnore]
        public Client Client { get; set; }


        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }


        [JsonIgnore]
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

        [JsonIgnore]
        public List<Mechanic> Mechanics { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Select at least 1 mechanic.")]
        public List<int> MechanicIds { get; set; }


        public List<Part> Parts { get; set; }


        [NotMapped] 
        public string TotalPrice
        {
            get
            {
                var partsTotal = Parts?.Sum(p => p.UnitPrice) ?? 0;
                return (ServicePrice + partsTotal).ToString("C");
            }
        }


        public string MechanicsNames => Mechanics != null
         ? string.Join(", ", Mechanics.Select(m => m.Name))
        : "N/A";


    }
}
