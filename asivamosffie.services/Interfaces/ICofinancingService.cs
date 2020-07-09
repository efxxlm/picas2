using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;

namespace asivamosffie.services.Interfaces
{
    public interface ICofinancingService
    { 
        Task<object> CreateorUpdateCofinancing(Cofinanciacion cofinanciacion);

        Task<List<Cofinanciacion>> GetListCofinancing();
       
    }
}
