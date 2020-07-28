using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectionProcessController : ControllerBase
    {
        private readonly ISelectionProcessService _selectionProcessService;
        private readonly IOptions<AppSettings> _settings;


        public SelectionProcessController(IOptions<AppSettings> settings, ISelectionProcessService selectionProcessService)
        {
            _selectionProcessService = selectionProcessService;
            _settings = settings;

        }

        [HttpGet]
        public async Task<ActionResult<List<ProcesoSeleccion>>> Get()
        {
            try
            {
                return await _selectionProcessService.GetSelectionProcess();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _selectionProcessService.GetSelectionProcessById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetSelectionProcessSchedule")]
        public async Task<IActionResult> GetSelectionProcessSchedule()
        {
            try
            {
                var result = await _selectionProcessService.GetSelectionProcessSchedule();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [Route("GetSelectionProcessScheduleById")]
        public async Task<IActionResult> GetSelectionProcessSchedule(int id)
        {
            try
            {
                var result = await _selectionProcessService.GetSelectionProcessScheduleById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetRecordActivities")]
        public async Task<IActionResult> GetRecordActivities(int ProcesoSeleccionId)
        {
            try
            {
                var result = await _selectionProcessService.GetRecordActivities(ProcesoSeleccionId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        [Route("GetControlGridSchedule")]
        public async Task<IActionResult> GetControlGridSchedule()
        {
            try
            {
                var result = await _selectionProcessService.GetControlGridSchedule();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateEditarProcesoSeleccion")]
        public async Task<IActionResult> CreateEditarProcesoSeleccion([FromBody] ProcesoSeleccion procesoSeleccion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                procesoSeleccion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _selectionProcessService.CreateEditarProcesoSeleccion(procesoSeleccion);
                return Ok(respuesta);
                //
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

    }
}
