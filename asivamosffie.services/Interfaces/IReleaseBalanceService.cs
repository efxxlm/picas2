using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IReleaseBalanceService
    {
        Task<List<dynamic>> GetDrpByProyectoId(int pProyectoId);
        Task<Respuesta> CreateEditHistoricalReleaseBalance(VUsosHistorico pUsosHistorico, string user);
        Task<Respuesta> ReleaseBalance(int pBalanceFinancieroId, string user);

    }
}
