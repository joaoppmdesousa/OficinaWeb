using OficinaWeb.Data.Entities;
using OficinaWeb.Models;
using Org.BouncyCastle.Asn1.Misc;

namespace OficinaWeb.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Appointment ToAppointment(AppointmentViewModel model, bool isNew)
        {
            return new Appointment
            {
                Id = isNew ? 0 : model.Id,
                AppointmentType = model.AppointmentType,
                Date = model.Date,
                AppointmentEnd = model.AppointmentEnd,
                ClientId = model.ClientId,
                Client = model.Client,
                MechanicId = model.MechanicId,
                Mechanic = model.Mechanic,
                VehicleId = model.VehicleId,
                Vehicle = model.Vehicle
            };
        }

        public AppointmentViewModel ToAppointmentViewModel(Appointment appointment)
        {
            return new AppointmentViewModel
            {
                Id = appointment.Id,
                AppointmentType = appointment.AppointmentType,
                Date = appointment.Date,
                AppointmentEnd = appointment.AppointmentEnd,
                ClientId = appointment.ClientId,
                Client = appointment.Client,
                MechanicId = appointment.MechanicId,
                Mechanic = appointment.Mechanic,
                VehicleId = appointment.VehicleId,
                Vehicle = appointment.Vehicle

            };
        }

        public RegisterNewUserViewModel ToRegisterNewUserViewModel(User user, string role)
        {
            return new RegisterNewUserViewModel
            {
                Name = user.Name,
                Username = user.UserName,
                Password = user.PasswordHash,
                ConfirmPassword = user.PasswordHash,
                Role = role
            };
        }


        public Vehicle ToVehicle(VehicleViewModel model, bool isNew)
        {
            return new Vehicle
            {
                Id = isNew ? 0 : model.Id,
                CarBrandId = model.CarBrandId,
                CarModelId = model.CarModelId,
                CarBrand = model.CarBrand,
                CarModel = model.CarModel,
                LicensePlate = model.LicensePlate,
                Year = model.Year,
                Mileage = model.Mileage,
                FuelType = model.FuelType,
                ClientId = model.ClientId,
                Client = model.Client

            };
        }

        public VehicleViewModel ToVehicleViewModel(Vehicle vehicle)
        {
            return new VehicleViewModel
            {
                Id = vehicle.Id,
                CarBrandId = vehicle.CarBrandId,
                CarModelId = vehicle.CarModelId,
                CarBrand = vehicle.CarBrand,
                CarModel = vehicle.CarModel,
                LicensePlate = vehicle.LicensePlate,
                Year = vehicle.Year,
                Mileage = vehicle.Mileage,
                FuelType = vehicle.FuelType,
                ClientId = vehicle.ClientId,
                Client = vehicle.Client

            };
        }


        public ScheduleViewModel ToScheduleViewModel(Appointment appointment, bool isEmployee, bool isClient, string currentUserEmail)
        {

            if (isEmployee)
            {

                return new ScheduleViewModel
                {
                    Id = appointment.Id,
                    Subject = $"{appointment.AppointmentType}, {appointment.Vehicle.VehicleDescription} ",
                    StartTime = appointment.Date,
                    EndTime = appointment.Date.Date + appointment.AppointmentEnd,
 

                };
            }

            if (isClient && appointment.Client?.Email == currentUserEmail)
            {
                return new ScheduleViewModel
                {
                    Id = appointment.Id,
                    Subject = $"My appointment: \n {appointment.AppointmentType}, \n {appointment.Mechanic.Name}, \n {appointment.Vehicle.VehicleDescription}",
                    StartTime = appointment.Date,
                    EndTime = appointment.Date.Date + appointment.AppointmentEnd,

                };
            }

            else
            {
                return new ScheduleViewModel
                {
                    Id = appointment.Id,
                    Subject = "Unavailable",
                    StartTime = appointment.Date,
                    EndTime = appointment.Date.Date + appointment.AppointmentEnd,
  

                };
            }

            
        }



        public RepairAndServices ToRepairAndServices(RepairAndServicesViewModel model, bool isNew)
        {
            return new RepairAndServices
            {
                Id = isNew ? 0 : model.Id,
                ServiceTypeId = model.ServiceTypeId,
                ServiceType = model.ServiceType,
                Details = model.Details,
                ClientId = model.ClientId,
                Client = model.Client,
                VehicleId = model.VehicleId,
                Vehicle = model.Vehicle,
                BeginDate = model.BeginDate,
                EndDate = model.EndDate,
                ServicePrice = model.ServicePrice,
                Mechanics = model.Mechanics,
                MechanicIds = model.MechanicIds,
                Parts = model.Parts


            };
        }

        public RepairAndServicesViewModel ToRepairAndServicesViewModel(RepairAndServices repairAndServices)
        {
            return new RepairAndServicesViewModel
            {
                Id = repairAndServices.Id,
                ServiceTypeId= repairAndServices.ServiceTypeId,
                ServiceType = repairAndServices.ServiceType,
                Details = repairAndServices.Details,
                ClientId = repairAndServices.ClientId,
                Client = repairAndServices.Client,
                VehicleId = repairAndServices.VehicleId,
                Vehicle = repairAndServices.Vehicle,
                BeginDate = repairAndServices.BeginDate,
                EndDate = repairAndServices.EndDate,
                ServicePrice = repairAndServices.ServicePrice,
                Mechanics = repairAndServices.Mechanics,
                MechanicIds = repairAndServices.MechanicIds,
                Parts = repairAndServices.Parts


            };
        }



    }
}
