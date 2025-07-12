using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using System;
using System.Linq;
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
            

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Client");
            await _userHelper.CheckRoleAsync("Employee");




            var user = await _userHelper.GetUserByEmailAsync("joaopedropsousa@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    Name = "joao",
                    Email = "joaopedropsousa@gmail.com",
                    UserName = "joaopedropsousa@gmail.com",
                    PhoneNumber = "236526854",
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }


            if (!_context.Specialties.Any())
            {
                _context.Specialties.Add(new MechanicSpecialty { Name = "Engine Specialist" });
                _context.Specialties.Add(new MechanicSpecialty { Name = "Brake Technician" });
                _context.Specialties.Add(new MechanicSpecialty { Name = "Electrical Specialist" });

                await _context.SaveChangesAsync();
            }

        }




    }
}
