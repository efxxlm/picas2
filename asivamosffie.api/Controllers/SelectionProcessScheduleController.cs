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
    public class SelectionProcessScheduleController : ControllerBase
    {
        private readonly ISelectionProcessScheduleService _selectionProcessScheduleService;
        private readonly IOptions<AppSettings> _settings;


        public SelectionProcessScheduleController(IOptions<AppSettings> settings, ISelectionProcessScheduleService selectionProcessScheduleService)
        {
            _selectionProcessScheduleService = selectionProcessScheduleService;
            _settings = settings;

        }

        [Route("GetListProcesoSeleccionCronogramaByProcesoSeleccionId")]
        [HttpGet]
        public async Task<ActionResult<List<ProcesoSeleccionCronograma>>> GetListProcesoSeleccionCronogramaBypProcesoSeleccionId(int pProcesoSeleccionId)
        {
            try
            {
                return await _selectionProcessScheduleService.GetListProcesoSeleccionCronogramaBypProcesoSeleccionId(pProcesoSeleccionId);
             }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ProcesoSeleccionCronograma>>> Get()
        {
            try
            {
                return await _selectionProcessScheduleService.GetSelectionProcessSchedule();
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
                var result = await _selectionProcessScheduleService.GetSelectionProcessScheduleById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> post([FromBody] ProcesoSeleccionCronograma procesoSeleccionCronograma)
        {
            Respuesta _response = new Respuesta();
            try
            {

                string usermodified = " ";
                //string usermodified = HttpContext.User.FindFirst("User").Value;
                procesoSeleccionCronograma.UsuarioCreacion = usermodified;
                _response = await _selectionProcessScheduleService.Insert(procesoSeleccionCronograma);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Data = ex.ToString();
                return Ok(_response);
            }
        }


        [Route("RecordActivities")]
        [HttpPost]
        public async Task<IActionResult> RecordActivities(ProcesoSeleccionObservacion procesoSeleccionObservacion)
        {
            Respuesta _response = new Respuesta();
            try
            {

                string usermodified = " ";
                //string usermodified = HttpContext.User.FindFirst("User").Value;
                procesoSeleccionObservacion.UsuarioCreacion = usermodified;
                _response = await _selectionProcessScheduleService.RecordActivities(procesoSeleccionObservacion);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Data = ex.ToString();
                return Ok(_response);
            }
        }


        [HttpPut]
        public async Task<IActionResult> update(ProcesoSeleccionCronograma procesoSeleccionCronograma)
        {
            Respuesta _reponse = new Respuesta();
            try
            {
                //procesoSeleccionCronograma.ProcesoSeleccionCronogramaId = id;
                _reponse = await _selectionProcessScheduleService.Update(procesoSeleccionCronograma);
                return Ok(_reponse);
            }
            catch (Exception ex)
            {
                _reponse.Data = ex.ToString();
                return Ok(_reponse);
            }   
        }
    }
}
