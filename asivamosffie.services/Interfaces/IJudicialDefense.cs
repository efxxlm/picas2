using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
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

        Task<DefensaJudicial> GetVistaDatosBasicosProceso(int pDefensaJudicialId = 0);
        Task<Respuesta> CreateOrEditFichaEstudio(FichaEstudio fichaEstudio);
        Task<Respuesta> CreateOrEditDefensaJudicial(DefensaJudicial defensaJudicial);
        Task<byte[]> GetPlantillaDefensaJudicial(int pContratoId);
        Task<Respuesta> CambiarEstadoDefensaJudicial(int pDefensaJudicialId, string pCodigoEstado, string pUsuarioModifica);
        Task<Respuesta> EliminarDefensaJudicial(int pDefensaJudicialId, string pUsuarioModifica);
        Task<List<ProyectoGrilla>> GetListProyects( int pProyectoId);
        Task<List<GrillaProcesoDefensaJudicial>> ListGrillaProcesosDefensaJudicial();
        Task<List<Contrato>> GetListContract();
        Task<List<ProyectoGrilla>> GetListProyectsByContract(int pContratoId);
        Task<List<DefensaJudicialSeguimiento>> GetActuacionesByDefensaJudicialID(int pDefensaJudicialId);
    }
}
