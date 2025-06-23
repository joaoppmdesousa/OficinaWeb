using Microsoft.AspNetCore.Identity;
using OficinaWeb.Data.Entities;
using System.Threading.Tasks;

namespace OficinaWeb.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);
    }
}
