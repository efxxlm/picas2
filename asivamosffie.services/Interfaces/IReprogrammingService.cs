using System.Collections.Generic;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.services.Interfaces
{
    public interface IReprogrammingService
    {
        Task<List<VAjusteProgramacion>> GetAjusteProgramacionGrid();
        Task<AjusteProgramacion> GetAjusteProgramacionById(int pAjusteProgramacionId);
        Task<Respuesta> AprobarAjusteProgramacion(int pAjusteProgramacionId, string pUsuarioCreacion, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
        Task<Respuesta> EnviarAlSupervisorAjusteProgramacion(int pAjusteProgramacionId, string pUsuarioCreacion);
        Task<Respuesta> CreateEditObservacionAjusteProgramacion(AjusteProgramacion pAjusteProgramacion, bool esObra, string pUsuario);
        Task<Respuesta> UploadFileToValidateAdjustmentProgramming(IFormFile pFile, string pFilePatch, string pUsuarioCreo, int pAjusteProgramacionId, int pContratacionProyectId, int pNovedadContractualId,
                                                                        int pContratoId, int pProyectoId);
        Task<Respuesta> TransferMassiveLoadAdjustmentProgramming(string pIdDocument, string pUsuarioModifico, int pProyectoId, int pContratoId);
        Task<Respuesta> UploadFileToValidateAdjustmentInvestmentFlow(IFormFile pFile, string pFilePatch, string pUsuarioCreo,
                                                                                int pAjusteProgramacionId, int pContratacionProyectId, int pNovedadContractualId,
                                                                                int pContratoId, int pProyectoId);
        Task<Respuesta> TransferMassiveLoadAdjustmentInvestmentFlow(string pIdDocument, string pUsuarioModifico, int pProyectoId, int pContratoId);
        Task<List<ArchivoCargue>> GetLoadAdjustProgrammingGrid(int pAjusteProgramacionId);
        Task<List<ArchivoCargue>> GetLoadAdjustInvestmentFlowGrid(int pAjusteProgramacionId);
        Task<Respuesta> DeleteAdjustProgrammingOrInvestmentFlow(int pArchivoCargueId, int pAjusteProgramacionId, string pUsuario);
        Task<Respuesta> CreateEditObservacionFile(AjusteProgramacion pAjusteProgramacion, bool esObra, string pUsuario);
        Task<Respuesta> EnviarAlInterventor(int pAjusteProgramacionId, string pUsuarioCreacion);

    }
}
