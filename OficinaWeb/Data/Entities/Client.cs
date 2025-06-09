using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Data.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Contact { get; set; }

        [Display(Name="Tax Number")]
        public int TaxNumber { get; set; }

        public string Email { get; set; }



    }
}
