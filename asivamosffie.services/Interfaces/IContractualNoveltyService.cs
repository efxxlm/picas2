using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IContractualNoveltyService
    {
        Task<List<CofinanciacionAportante>> GetAportanteByNovedadContractualId(int pNovedadContractualId);
        Task<List<CofinanciacionAportante>> GetAportanteByContratacion(int pId);

        Task<Respuesta> CreateEditNovedadContractual(NovedadContractual novedadContractual);
        Task<List<VNovedadContractual>> GetListGrillaNovedadContractualObra();
        Task<List<VNovedadContractual>> GetListGrillaNovedadContractualInterventoria();
        Task<Respuesta> EliminarNovedadContractual(int pNovedadContractualId, string pUsuario);
        Task<Respuesta> EliminarNovedadClausula(int pNovedadContractuaClausulalId, string pUsuario);
        Task<List<Contrato>> GetListContract(int userID);
        Task<List<VProyectosXcontrato>> GetProyectsByContract(int pContratoId);
        Task<NovedadContractual> GetNovedadContractualById(int pId);
        Task<Respuesta> AprobarSolicitud(int pNovedadContractualId, string pUsuario);
        Task<Respuesta> EnviarAlSupervisor(int pNovedadContractualId, string pUsuario);
        Task<Respuesta> CreateEditObservacion(NovedadContractual pNovedadContractual, bool? esSupervisor, bool? esTramite);
        Task<Respuesta> TramitarSolicitud(int pNovedadContractualId, string pUsuario);
        Task<Respuesta> DevolverSolicitud(int pNovedadContractualId, string pUsuario);
        Task<Respuesta> DevolverSolicitudASupervisor(int pNovedadContractualId, string pUsuario);
        Task<List<VNovedadContractual>> GetListGrillaNovedadContractualGestionar();
        Task<Respuesta> CreateEditNovedadContractualTramite(NovedadContractual novedadContractual);
        Task<Respuesta> EnviarAComite(int pNovedadContractualId, string pUsuario);
        Task<Respuesta> RechazarPorInterventor(NovedadContractual pNovedadContractual, string pUsuario);
        Task<Respuesta> RechazarPorSupervisor(NovedadContractual pNovedadContractual, string pUsuario); 
        Task<List<FuenteFinanciacion>> GetFuentesByAportante(int pConfinanciacioAportanteId);
        Task<Respuesta> CancelarNovedad(int pNovedadContractualId, string pUsuario);
        Task<Respuesta> AprobacionTecnicaJuridica(int pNovedadContractualId, string pUsuario);
        Task<Respuesta> EliminarNovedadContractualAportante(int pNovedadContractualAportante, string pUsuario);
        Task<Respuesta> EliminarComponenteAportanteNovedad(int pComponenteAportanteNovedad, string pUsuario);
        Task<Respuesta> EliminarComponenteFuenteNovedad(int pComponenteFuenteNovedad, string pUsuario);
        Task<Respuesta> EliminarComponenteUsoNovedad(int pComponenteUsoNovedad, string pUsuario);
        Task<List<dynamic>> GetDatosContratoProyectoModificadosXNovedad(int pProyectoId, int pContratoId);
    }
}
