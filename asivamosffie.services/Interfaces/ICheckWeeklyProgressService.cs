using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface ICheckWeeklyProgressService
    {
        Task<bool> GetValidarRegistroCompletoObservaciones(int pSeguimientoSemanalId, bool esSupervisor);

        Task<List<VVerificarValidarSeguimientoSemanal>> GetListReporteSemanalView(List<string> strListCodEstadoSeguimientoSemanal);

        Task<dynamic> GetListReporteSemanal();

        Task<Respuesta> CreateEditSeguimientoSemanalObservacion(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion);

        Task<SeguimientoSemanal> GetSeguimientoSemanalBySeguimientoSemanalId(int pSeguimientoSemanalId);
    }
}
