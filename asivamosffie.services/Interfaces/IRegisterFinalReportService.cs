using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;


namespace asivamosffie.services.Interfaces
{
    public interface IRegisterFinalReportService
    {
        //Consultas
        Task<List<VProyectosCierre>> gridRegisterFinalReport();
        Task<List<dynamic>> GetInformeFinalListaChequeoByProyectoId(int pProyectoId);
        Task<List<dynamic>> GetInformeFinalByProyectoId(int pProyectoId);
        Task<InformeFinalInterventoria> GetInformeFinalAnexoByInformeFinalInterventoriaId(int pInformeFinalInterventoriaId);
        Task<InformeFinalAnexo> GetInformeFinalAnexoByInformeFinalAnexoId(int pInformeFinalAnexoId);
        Task<InformeFinalInterventoria> GetObservacionesByInformeFinalInterventoriaId(int pInformeFinalInterventoriaId);

        Task<InformeFinal> GetInformeFinalByInformeFinalId(int pInformeFinalId);
        Task<InformeFinalInterventoriaObservaciones> GetInformeFinalInterventoriaObservacionByInformeFinalObservacion(int pObservacionId);

        //Creaci?n y edici?n
        Task<Respuesta> CreateEditInformeFinal(InformeFinal pInformeFinal);
        Task<Respuesta> CreateEditInformeFinalInterventoria(InformeFinalInterventoria pInformeFinalInterventoriaId);
        Task<Respuesta> CreateEditInformeFinalInterventoriabyInformeFinal(InformeFinal pInformeFinal,string user);
        Task<Respuesta> CreateEditInformeFinalAnexo(InformeFinalAnexo pInformeFinalAnexoId, int pInformeFinalInterventoriaId);
        Task<Respuesta> CreateEditInformeFinalInterventoriaObservacion(InformeFinalInterventoriaObservaciones pObservacion);
        Task<Respuesta> SendFinalReportToSupervision(int pProyectoId, string pUsuario);

        //Alertas
        Task<bool> GetInformeFinalSinGestionar();
    }
}
