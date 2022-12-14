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

        Task<List<VVerificarSeguimientoSemanal>> GetListReporteSemanalView(List<string> strListCodEstadoSeguimientoSemanal, int pUserId);

        Task<dynamic> GetListReporteSemanal();

        Task<Respuesta> CreateEditSeguimientoSemanalObservacion(SeguimientoSemanalObservacion pSeguimientoSemanalObservacion , bool pEliminarRegistroCompleto);

        Task<SeguimientoSemanal> GetSeguimientoSemanalBySeguimientoSemanalId(int pSeguimientoSemanalId);
    }
}
