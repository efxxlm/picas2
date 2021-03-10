using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Interfaces
{
    public interface IValidateFulfilmentFinalReportService
    {
        Task<List<InformeFinal>> GetListInformeFinal();
        Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId);
        Task<List<InformeFinalInterventoria>> GetInformeFinalListaChequeoByInformeFinalId(int pInformeFinalId);
        //POST
        Task<Respuesta> CreateEditObservacionInformeFinal(InformeFinalObservaciones pObseravacion, bool tieneObservacion);
        Task<Respuesta> CreateEditObservacionInformeFinalInterventoria(InformeFinalObservaciones pObseravacion, bool tieneObservacion);
        Task<Respuesta> SendFinalReportToSupervision(int pInformeFinalId, string pUsuario, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
        Task<Respuesta> ApproveFinalReportByFulfilment(int pInformeFinalId, string pUsuario);

        //Alertas
        Task GetInformeFinalNoCumplimiento(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);

    }
}
