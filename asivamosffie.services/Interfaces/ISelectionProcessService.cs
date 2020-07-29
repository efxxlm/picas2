﻿using asivamosffie.model.APIModels;
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
        Task<ActionResult<List<GrillaControlCronograma>>> GetControlGridSchedule();
        Task<ProcesoSeleccionCronograma> GetSelectionProcessScheduleById(int id);
        Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetRecordActivities(int ProcesoSeleccionId);
        Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetScheduleBySelectionProcessId(int ProcesoSeleccionId);
        Task<Respuesta> CreateEditarProcesoSeleccionGrupo(ProcesoSeleccionGrupo procesoSeleccionGrupo);

        //Cotizacion
        Task<ProcesoSeleccionCotizacion> GetProcesoSeleccionCotizacionById(int id);
        Task<ActionResult<List<ProcesoSeleccionCotizacion>>> GetProcesoSeleccionCotizacion();
        Task<ActionResult<List<ProcesoSeleccionCotizacion>>> GetCotizacionByProcesoSeleccionId(int ProcesoSeleccionId);
        Task<Respuesta> CreateEditarProcesoSeleccionCotizacion(ProcesoSeleccionCotizacion procesoSeleccionCotizacion);
        Task<bool> Delete(int id);
    }
}
