using Microsoft.AspNetCore.Identity;
using OficinaWeb.Data.Entities;
using OficinaWeb.Models;
using System.Threading.Tasks;

namespace OficinaWeb.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();
    }
}
