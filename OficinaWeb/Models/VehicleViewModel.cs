using Microsoft.AspNetCore.Mvc.Rendering;
using OficinaWeb.Data.Entities;
using System.Collections.Generic;

namespace OficinaWeb.Models
{
    public class VehicleViewModel : Vehicle
    {
        public IEnumerable<SelectListItem> Clients { get; set; }

        public Vehicle ToVehicle()
        {
            return new Vehicle
            {
                Id = this.Id,
                LicensePlate = this.LicensePlate,
                Brand = this.Brand,
                Model = this.Model,
                Year = this.Year,
                Mileage = this.Mileage,
                FuelType = this.FuelType,
                ClientId = this.ClientId
            };
        }
    }


}
