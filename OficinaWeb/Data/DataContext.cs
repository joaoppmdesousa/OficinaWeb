using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data.Entities;

namespace OficinaWeb.Data
{
    public class DataContext : DbContext
    {

        public DbSet<Client> Clients { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {                
        }

    }
}
