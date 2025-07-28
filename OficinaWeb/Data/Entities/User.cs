using Microsoft.AspNetCore.Identity;

namespace OficinaWeb.Data.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public string? ImageUrl { get; set; }
    }
}
