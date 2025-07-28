using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OficinaWeb.Data.Entities
{
    public class Part : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Price")]
        public decimal UnitPrice { get; set; }

        public int RepairAndServicesId { get; set; }

        [JsonIgnore]
        public RepairAndServices RepairAndServices { get; set; }



    }
}
