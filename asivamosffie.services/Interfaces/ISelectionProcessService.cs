using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace asivamosffie.services.Interfaces
{
    public interface ISelectionProcessService
    {
        Task<ActionResult<List<ProcesoSeleccion>>> GetSelectionProcess();
        Task<ProcesoSeleccion> GetSelectionProcessById(int id);
        Task<Respuesta> CreateEditarProcesoSeleccion(ProcesoSeleccion procesoSeleccion);
        Task<Respuesta> CreateEditarProcesoSeleccionCronograma(ProcesoSeleccionCronograma procesoSeleccionCronograma);
        Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetSelectionProcessSchedule();
        Task<ProcesoSeleccionCronograma> GetSelectionProcessScheduleById(int id);
        Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetRecordActivities(int ProcesoSeleccionId);

        Task<bool> Delete(int id);
    }
}
