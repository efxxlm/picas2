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
        Task<List<VProyectosCierre>> gridRegisterFinalReport();
        Task<List<ContratacionProyecto>> GetInformeFinalByContratacionProyectoId(int pContratacionProyectoId);
        Task<Respuesta> CreateEditInformeFinal( InformeFinal pInformeFinal );
        Task<List<dynamic>> GetInformeFinalListaChequeoByInformeFinalInterventoriaId(int pInformeFinalInterventoriaId);

    }
}
