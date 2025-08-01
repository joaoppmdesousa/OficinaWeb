﻿using Microsoft.AspNetCore.Mvc.Rendering;
using OficinaWeb.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaWeb.Data
{
    public interface IMechanicRepository : IGenericRepository<Mechanic>
    {

        Task<List<Mechanic>> GetByIdsAsync(IEnumerable<int> ids);

        public Task<Mechanic> GetByIdAsyncWithIncludes(int id);


        Task<Mechanic> GetByEmailAsync(string email);


        IEnumerable<SelectListItem> GetComboProducts();

    }
}
