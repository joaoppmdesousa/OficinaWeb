using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OficinaWeb.Data.Entities;
using System.Linq;
using System.Reflection.Emit;

namespace OficinaWeb.Data
{
    public class DataContext : IdentityDbContext<User>
    {

        public DbSet<Client> Clients { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Mechanic> Mechanics { get; set; }

        public DbSet<RepairAndServices> RepairsAndServices { get; set; }

        public DbSet<Part> Parts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
           
        {
                  
            var cascadeFKs = modelBuilder.Model
            
                .GetEntityTypes()
                
                .SelectMany(t => t.GetForeignKeys())
                
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
                foreach (var fk in cascadeFKs)
                {
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
                }

                modelBuilder.Entity<RepairAndServices>()
               .HasOne(r => r.Client)
               .WithMany(c => c.RepairsAndServices) 
               .HasForeignKey(r => r.ClientId)
               .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Part>()
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<RepairAndServices>()
               .Property(p => p.ServicePrice)
               .HasColumnType("decimal(18,2)");


            base.OnModelCreating(modelBuilder);
            
        }
            
        public DbSet<OficinaWeb.Data.Entities.Appointment> Appointment { get; set; }
     


    }
}
