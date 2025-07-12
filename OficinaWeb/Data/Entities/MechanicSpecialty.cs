using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Data.Entities
{
    public class MechanicSpecialty : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
       
    }
}
