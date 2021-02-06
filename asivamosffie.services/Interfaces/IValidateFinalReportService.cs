using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IValidateFinalReportService
    {
        Task<List<InformeFinal>> GetListInformeFinal();
        Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId);
        Task<List<InformeFinalInterventoria>> GetInformeFinalListaChequeoByInformeFinalId(int pInformeFinalId);
        Task<bool> VerificarInformeFinalValidacion(int pInformeFinalId);
        Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalObservacion(int pObservacionId);
        Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalInterventoria(int pInformeFinalInterventoriaId);
        //POST
        Task<Respuesta> UpdateStateValidateInformeFinalInterventoria(int pInformeFinalInterventoriaId, string code, string user);
        Task<Respuesta> CreateEditInformeFinalInterventoriaObservacion(InformeFinalInterventoriaObservaciones pObservacion);
        Task<Respuesta> UpdateInformeFinalObservacion(InformeFinal pinformeFinal);
        Task<Respuesta> SendFinalReportToSupervision(int pInformeFinalId, string pUsuario);
        Task<Respuesta> ApproveInformeFinal(int pInformeFinalId, string pUsuario);
        Task<Respuesta> NoApprovedInformeFinal(int pInformeFinalId, string pUsuario);

    }
}
