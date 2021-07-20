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
        Task<byte[]> GetPlantillaDefensaJudicial(int pContratoId, int tipoArchivo);
        Task<Respuesta> CambiarEstadoDefensaJudicial(int pDefensaJudicialId, string pCodigoEstado, string pUsuarioModifica);
        Task<Respuesta> EliminarDefensaJudicial(int pDefensaJudicialId, string pUsuarioModifica);
        Task<List<ProyectoGrilla>> GetListProyects(int pProyectoId);
        Task<List<GrillaProcesoDefensaJudicial>> ListGrillaProcesosDefensaJudicial();
        Task<dynamic> GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato(string pTipoSolicitud, string pModalidadContrato, string pNumeroContrato);
        Task<List<Contrato>> GetListContract();
        Task<List<ProyectoGrilla>> GetListProyectsByContract(int pContratoId);
        Task<List<DefensaJudicialSeguimiento>> GetActuacionesByDefensaJudicialID(int pDefensaJudicialId);
        Task<Respuesta> EnviarAComite(int pDefensaJudicialId, string pUsuarioModifico);
        Task<Respuesta> DeleteActuation(int pDefensaJudicialId, string pUsuarioModifico);
        Task<Respuesta> FinalizeActuation(int pDefensaJudicialId, string pUsuarioModifico);
        Task<Respuesta> CerrarProceso(int pDefensaJudicialId, string pUsuarioModifico);
        Task<Respuesta> CreateOrEditDefensaJudicialSeguimiento(DefensaJudicialSeguimiento defensaJudicialSeguimiento);
        Task<DefensaJudicialSeguimiento> GetDefensaJudicialSeguimiento(int defensaJudicialSeguimientoId);
        Task<Respuesta> DeleteDemandadoConvocado(int demandadoConvocadoId, string pUsuarioModificacion, int numeroDemandados);
        Task<Respuesta> DeleteDefensaJudicialContratacionProyecto(int contratacionId, int defensaJudicialId, string pUsuarioModificacion, int cantContratos);
        Task VencimientoTerminosDefensaJudicial();
        Task<Respuesta> DeleteDemandanteConvocante(int demandanteConvocadoId, string pUsuarioModificacion, int numeroDemandantes);
        Task<string> ReemplazarDatosPlantillaDefensaJudicial(string strContenido, int prmdefensaJudicialID, int tipoArchivo);

    }
}
