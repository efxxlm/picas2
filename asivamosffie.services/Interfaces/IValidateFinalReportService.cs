﻿using System;
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
        Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalObservacion(int pObservacionId);
        Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalInterventoria(int pInformeFinalInterventoriaId);
        //POST
        Task<Respuesta> UpdateStateApproveInformeFinalInterventoriaByInformeFinal(InformeFinal informeFinal, string user);
        Task<Respuesta> UpdateStateApproveInformeFinalInterventoria(int pInformeFinalInterventoriaId, string code, string user);
        Task<Respuesta> CreateEditObservacionInformeFinal(InformeFinalObservaciones pObseravacion, bool tieneObservacion);
        Task<Respuesta> CreateEditInformeFinalInterventoriaObservacion(InformeFinalInterventoriaObservaciones pObservacion);
        Task<Respuesta> SendFinalReportToInterventor(int pInformeFinalId, string pUsuario, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
        Task<Respuesta> SendFinalReportToFinalVerification(int pInformeFinalId, string pUsuario, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);
        //alertas
        Task GetInformeFinalNoEnviadoAGrupoNovedades(string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSender);

    }
}
