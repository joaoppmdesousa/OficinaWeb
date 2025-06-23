using Microsoft.AspNetCore.Identity;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using System;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            var user = await _userHelper.GetUserByEmailAsync("joaopedropsousa@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    Name = "admin",
                    Email = "joaopedropsousa@gmail.com",
                    UserName = "joaopedropsousa@gmail.com",
                    PhoneNumber = "236526854",
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

        }




    }
}
