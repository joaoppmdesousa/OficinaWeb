using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Data.Entities
{
    public class Part : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Price")]
        public decimal UnitPrice { get; set; }

        public int RepairAndServicesId { get; set; }

        public RepairAndServices RepairAndServices { get; set; }



    }
}
