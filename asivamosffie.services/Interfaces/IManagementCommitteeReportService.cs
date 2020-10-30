using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface IManagementCommitteeReportService
    {
        //Tarea Programada
        Task GetApproveExpiredMinutes(string pServerUser);

         
        Task<List<dynamic>> GetListCompromisos(int pUserId);
        Task<Respuesta> ChangeStatusSesionComiteSolicitudCompromiso(SesionSolicitudCompromiso pSesionSolicitudCompromiso);
        Task<ActionResult<List<GrillaSesionComiteTecnicoCompromiso>>> GetManagementCommitteeReport(int pUserId); 
        Task<ActionResult<List<GrillaSesionComiteTecnicoCompromiso>>> GetManagementCommitteeReportById(int sesionComiteTecnicoCompromisoId);
        Task<Respuesta> CreateOrEditReportProgress(CompromisoSeguimiento compromisoSeguimiento, string estadoCompromiso);
        Task<Respuesta> CreateOrEditCommentReport(SesionComentario SesionComentario);
        Task<ActionResult<List<ComiteTecnico>>> GetManagementReport(int pUserId);
        Task<ActionResult<List<ComiteTecnico>>> GetManagementReportById(int comiteTecnicoId);
        //Task<ActionResult<List<ComiteTecnico>>> GetManagementReport(int comiteTecnicoId);
        Task<bool> UpdateStatus(int sesionComiteTecnicoCompromisoId, string status);
        Task<Respuesta> AcceptReport(int comiteTecnicoId, Usuario puser,string pDominioFront, string pMailServer, int pMailPort,bool pEnableSSL, string pPassword,string pSender);
        Task<HTMLContent> GetHTMLString(ActaComite obj);
        Task<List<dynamic>> GetListCompromisoSeguimiento(int SesionSolicitudCompromisoId ,int pTipoCompromiso); 

    }
    
}
