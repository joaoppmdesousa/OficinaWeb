using OficinaWeb.Data.Entities;
using OficinaWeb.Models;
using System;

namespace OficinaWeb.Helpers
{
    public interface IConverterHelper
    {

        Appointment ToAppointment(AppointmentViewModel model, bool isNew);

        AppointmentViewModel ToAppointmentViewModel(Appointment appointment);


        RegisterNewUserViewModel ToRegisterNewUserViewModel(User user, string role);


        Vehicle ToVehicle(VehicleViewModel model, bool isNew);

        VehicleViewModel ToVehicleViewModel(Vehicle vehicle);

        ScheduleViewModel ToScheduleViewModel(Appointment appointment);

        RepairAndServices ToRepairAndServices(RepairAndServicesViewModel model, bool isNew);

        RepairAndServicesViewModel ToRepairAndServicesViewModel(RepairAndServices repairAndServices);

    }
}
