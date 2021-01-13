using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IValidateWeeklyProgressService
    { 
        Task<List<VVerificarValidarSeguimientoSemanal>> GetListReporteSemanalView(List<string> strListCodEstadoSeguimientoSemanal);

        Task<Respuesta> ReturnSeguimientoSemanal(int pSeguimientoSemanalId, string pUsuarioMod);
    }
}
