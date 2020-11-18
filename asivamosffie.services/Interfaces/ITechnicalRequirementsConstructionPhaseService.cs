using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface ITechnicalRequirementsConstructionPhaseService
    {
        Task<Respuesta> CreateEditObservacionConstruccionPerfilSave(ConstruccionPerfilObservacion pObservacion, string pUsuarioCreacion);
        Task<Respuesta> CambiarEstadoContratoEstadoVerificacionConstruccionCodigo(int ContratoId, string pEstado, string pUsuarioMod);
        Task<Respuesta> CreateEditObservacion(ContratoConstruccion pContratoConstruccion, string pTipoObservacion, bool pEsSupervicion);
        Task<List<dynamic>> GetContractsGrid(int pUsuarioId);
        Task<Contrato> GetContratoByContratoId(int pContratoId, string pUsuarioCreacion);
        Task<Respuesta> CreateEditDiagnostico(ContratoConstruccion pConstruccion);
        Task<Respuesta> CreateEditPlanesProgramas(ContratoConstruccion pConstruccion);
        Task<Respuesta> CreateEditManejoAnticipo(ContratoConstruccion pConstruccion);
        Task<Respuesta> CreateEditConstruccionPerfil(ContratoConstruccion pConstruccion);
        Task<Respuesta> DeleteConstruccionPerfil(int pConstruccionPerfilId, string pUsuarioModificacion);
        Task<Respuesta> DeleteConstruccionPerfilNumeroRadicado(int pConstruccionPerfilNumeroRadicadoId, string pUsuarioModificacion);
        Task<Respuesta> UploadFileToValidateProgramming(IFormFile pFile, string pFilePatch, string pUsuarioCreo, int pContratoConstruccionId);
        Task<Respuesta> TransferMassiveLoadProgramming(string pIdDocument, string pUsuarioModifico);
        Task<Respuesta> UploadFileToValidateInvestmentFlow(IFormFile pFile, string pFilePatch, string pUsuarioCreo, int pContratoConstruccionId);
        Task<Respuesta> TransferMassiveLoadInvestmentFlow(string pIdDocument, string pUsuarioModifico);
        Task<List<ArchivoCargue>> GetLoadProgrammingGrid(int pContratoConstruccionId);
        Task<List<ArchivoCargue>> GetLoadInvestmentFlowGrid(int pContratoConstruccionId);
        Task<Respuesta> CreateEditObservacionesCarga(int pArchivoCargueId, string pObservacion, string pUsuarioCreacion);
        Task<Respuesta> DeleteArchivoCargue(int pArchivocargue, int pContratoConstruccionId, bool pEsFlujoInvserion, string pUsuarioModificacion);
        Task<Byte[]> GetPDFDRP(int pContratoId, string usuarioModificacion);
        Task<Respuesta> CreateEditObservacionDiagnostico(ContratoConstruccion pContratoConstruccion, bool esSupervisor);
        Task<List<dynamic>> GetContractsGridApoyoObra();
        Task<List<dynamic>> GetContractsGridApoyoInterventoria();
        Task<Respuesta> CreateEditObservacionPlanesProgramas(ContratoConstruccion pContratoConstruccion, bool esSupervisor);
        Task<Respuesta> CreateEditObservacionManejoAnticipo(ContratoConstruccion pContratoConstruccion, bool esSupervisor);
        Task<Respuesta> CreateEditObservacionProgramacionObra(ContratoConstruccion pContratoConstruccion, bool esSupervisor);
        Task<Respuesta> CreateEditObservacionFlujoInversion(ContratoConstruccion pContratoConstruccion, bool esSupervisor);
        Task<Respuesta> EnviarAlSupervisor(int pContratoId, string pUsuarioCreacion);
        Task<Respuesta> CreateEditObservacionPerfil(ConstruccionPerfil pPerfil, bool esSupervisor);
        Task<Respuesta> AprobarInicio(int pContratoId, string pUsuarioCreacion);

    }
}
