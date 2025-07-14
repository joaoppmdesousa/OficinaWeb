using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using System;
using System.Collections.Generic;
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

            await _context.SaveChangesAsync();

            if (!_context.Specialties.Any())
            {
                _context.Specialties.Add(new MechanicSpecialty { Name = "Engine Specialist" });
                _context.Specialties.Add(new MechanicSpecialty { Name = "Brake Technician" });
                _context.Specialties.Add(new MechanicSpecialty { Name = "Electrical Specialist" });

                await _context.SaveChangesAsync();
            }

            if (!_context.CarBrands.Any())
            {
                var bmw = new CarBrand { Name = "BMW" };
                var toyota = new CarBrand { Name = "Toyota" };
                var ford = new CarBrand { Name = "Ford" };

                _context.CarBrands.AddRange(bmw, toyota, ford);
                await _context.SaveChangesAsync();

                // Agora adiciona os modelos para cada marca
                var bmwModels = new List<CarModel>
                {
                    new CarModel { Name = "Serie 1", CarBrand = bmw },
                    new CarModel { Name = "Serie 3", CarBrand = bmw },
                    new CarModel { Name = "X5", CarBrand = bmw }
                };

                var toyotaModels = new List<CarModel>
                {
                    new CarModel { Name = "Corolla", CarBrand = toyota },
                    new CarModel { Name = "Yaris", CarBrand = toyota },
                    new CarModel { Name = "RAV4", CarBrand = toyota }
                };

                var fordModels = new List<CarModel>
                 {
                    new CarModel { Name = "Fiesta", CarBrand = ford },
                    new CarModel { Name = "Focus", CarBrand = ford },
                    new CarModel { Name = "Mustang", CarBrand = ford }
                };

                _context.CarModels.AddRange(bmwModels);
                _context.CarModels.AddRange(toyotaModels);
                _context.CarModels.AddRange(fordModels);

                await _context.SaveChangesAsync();             


            }

            if (!_context.ServiceTypes.Any())
            {
                _context.ServiceTypes.AddRange(
                    new ServiceType { Name = "Oil Change" },
                    new ServiceType { Name = "Tire Rotation" },
                    new ServiceType { Name = "Brake Inspection" },
                    new ServiceType { Name = "Engine Diagnostics" },
                    new ServiceType { Name = "Vehicle Inspection" },
                    new ServiceType { Name = "Battery Replacement" },
                    new ServiceType { Name = "Wheel Alignment" },
                    new ServiceType { Name = "Air Conditioning Service" },
                    new ServiceType { Name = "Transmission Repair" },
                    new ServiceType { Name = "Exhaust System Repair" },
                    new ServiceType { Name = "Suspension Repair" }
                );

                await _context.SaveChangesAsync();
            }


        }

    }
}
