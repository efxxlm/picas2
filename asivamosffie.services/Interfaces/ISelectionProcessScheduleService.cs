using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface ISelectionProcessScheduleService
    {
        Task<List<ProcesoSeleccionCronograma>> GetListProcesoSeleccionCronogramaBypProcesoSeleccionId(int pProcesoSeleccionId);


        Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetSelectionProcessSchedule();
        Task<ProcesoSeleccionCronograma> GetSelectionProcessScheduleById(int id);

        Task<ActionResult<List<ProcesoSeleccionObservacion>>> GetRecordActivities();
        Task<Respuesta> RecordActivities(ProcesoSeleccionObservacion pocesoSeleccionObservacion);

        Task<Respuesta> Insert(ProcesoSeleccionCronograma procesoSeleccionCronograma);
        Task<Respuesta> Update(ProcesoSeleccionCronograma procesoSeleccionCronograma);
        Task<bool> Delete(int id);
        Task<ActionResult<List<ProcesoSeleccionMonitoreo>>> GetListProcesoSeleccionMonitoreoCronogramaByProcesoSeleccionId(int pProcesoSeleccionId);
        Task<Respuesta> setProcesoSeleccionMonitoreoCronograma(ProcesoSeleccionMonitoreo procesoSeleccionCronograma, string pDominioFront, string pMailServer, int pMailPort, bool pEnableSSL, string pPassword, string pSentender);
        Task<ActionResult<List<ProcesoSeleccionCronogramaMonitoreo>>> GetListProcesoSeleccionMonitoreoCronogramaByMonitoreoId(int pProcesoSeleccionId);
    }
}
