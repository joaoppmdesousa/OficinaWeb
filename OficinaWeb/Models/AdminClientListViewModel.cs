using OficinaWeb.Data.Entities;
using System.Collections.Generic;

namespace OficinaWeb.Models
{
    public class AdminClientListViewModel
    {
        public List<Client> ClientsWithoutUser { get; set; }

        public List<Client> ClientsWithUser { get; set; }
    }
}
