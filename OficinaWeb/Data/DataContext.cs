using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data.Entities;

namespace OficinaWeb.Data
{
    public class DataContext : IdentityDbContext<User>
    {

        public DbSet<Client> Clients { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Mechanic> Mechanics { get; set; }

        public DbSet<RepairAndServices> RepairsAndServices { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {                
        }

    }
}
