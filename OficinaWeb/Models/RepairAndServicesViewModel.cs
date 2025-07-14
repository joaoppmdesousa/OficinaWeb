using Microsoft.AspNetCore.Mvc.Rendering;
using OficinaWeb.Data.Entities;
using System.Collections.Generic;

namespace OficinaWeb.Models
{
    public class RepairAndServicesViewModel : RepairAndServices
    {
        public SelectList ServiceTypes { get; set; } 
    }
}
