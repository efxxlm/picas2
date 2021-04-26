using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IVerifyFinalReportService
    {
        Task<List<InformeFinal>> GetListInformeFinal();
        Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId);
        Task<List<InformeFinalInterventoria>> GetInformeFinalListaChequeoByInformeFinalId(int pInformeFinalId);
        Task<bool> VerificarInformeFinalValidacion(int pInformeFinalId);
        Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalObservacion(int pObservacionId);
        Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalInterventoria(int pInformeFinalInterventoriaId);
        //POST
        Task<Respuesta> UpdateStateValidateInformeFinalInterventoriaByInformeFinal(InformeFinal informeFinal, string user);
        Task<Respuesta> UpdateStateValidateInformeFinalInterventoria(int pInformeFinalInterventoriaId, string code, string user, bool tieneModificacionApoyo);
        Task<Respuesta> CreateEditObservacionInformeFinal(InformeFinalObservaciones pObseravacion, bool tieneObservacion);
        Task<Respuesta> CreateEditInformeFinalInterventoriaObservacion(InformeFinalInterventoriaObservaciones pObservacion);
        Task<Respuesta> SendFinalReportToSupervision(int pProyectoId, string pUsuario);
        Task<Respuesta> ApproveInformeFinal(int pInformeFinalId, string pUsuario);
        Task<Respuesta> NoApprovedInformeFinal(int pInformeFinalId, string pUsuario);

        //Alertas
        Task<bool> GetInformeFinalNoEnviadoASupervisor();


    }
}
