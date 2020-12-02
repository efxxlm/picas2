using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IJudicialDefense
    {
        Task<Respuesta> CreateOrEditDemandadoConvocado(DemandadoConvocado demandadoConvocado);
        Task<string> GetNombreContratistaByContratoId(int pContratoId);

        Task<Respuesta> CreateOrEditFichaEstudio(FichaEstudio fichaEstudio);
        Task<Respuesta> CreateOrEditDefensaJudicial(DefensaJudicial defensaJudicial);

        Task<List<ProyectoGrilla>> GetListProyects( int pProyectoId);
        Task<List<GrillaProcesoDefensaJudicial>> ListGrillaProcesosDefensaJudicial();
    }
}
