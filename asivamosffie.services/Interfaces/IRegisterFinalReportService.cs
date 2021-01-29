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
        Task<List<dynamic>> GetInformeFinalListaChequeoByContratacionProyectoId(int pContratacionProyectoId);
        Task<List<ContratacionProyecto>> GetInformeFinalByContratacionProyectoId(int pContratacionProyectoId);
        Task<InformeFinalInterventoria> GetInformeFinalAnexoByInformeFinalInterventoriaId(int pInformeFinalInterventoriaId);

        //Creación y edición
        Task<Respuesta> CreateEditInformeFinal(InformeFinal pInformeFinal);
        Task<Respuesta> CreateEditInformeFinalInterventoria(InformeFinalInterventoria pInformeFinalInterventoriaId);
        Task<Respuesta> CreateEditInformeFinalAnexo(InformeFinalAnexo pInformeFinalAnexoId, int pInformeFinalInterventoriaId);
        Task<Respuesta> CreateEditInformeFinalInterventoriaObservacion(InformeFinalInterventoriaObservaciones pObservacion);

        //Eliminaciones

    }
}
