using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;

namespace asivamosffie.services.Interfaces
{
    public interface ICofinancingService
    { 
        Task<object> CreateCofinancing(Cofinanciacion cofinanciacion, List<CofinanciacionAportante> pListCofinanciacionAportante, List<CofinanciacionDocumento> pListconinanciacionDocumentos);

        Task<List<Cofinanciacion>> GetListCofinancing();

    }
}
