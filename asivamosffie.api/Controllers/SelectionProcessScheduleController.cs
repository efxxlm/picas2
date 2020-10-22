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
                string usermodified = HttpContext.User.FindFirst("User").Value.ToUpper();
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

                string usermodified = HttpContext.User.FindFirst("User").Value.ToUpper();
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

        /*autor: jflorez
            descripción: trae las actividades por proceso de selección
            impacto: CU 3.1.3*/
        [Route("GetListProcesoSeleccionMonitoreoCronogramaByProcesoSeleccionId")]
        [HttpGet]
        public async Task<ActionResult<List<ProcesoSeleccionMonitoreo>>> GetListProcesoSeleccionMonitoreoCronogramaByProcesoSeleccionId(int pProcesoSeleccionId)
        {
            try
            {
                return await _selectionProcessScheduleService.GetListProcesoSeleccionMonitoreoCronogramaByProcesoSeleccionId(pProcesoSeleccionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*autor: jflorez
                descripción: crea o actualiza el monitoreo a cronograma
                impacto: CU 3.1.3*/
        [HttpPost]
        [Route("setProcesoSeleccionMonitoreoCronograma")]
        public async Task<IActionResult> setProcesoSeleccionMonitoreoCronograma([FromBody] ProcesoSeleccionMonitoreo procesoSeleccionCronograma)
        {
            Respuesta _response = new Respuesta();
            try
            {

                string usermodified = HttpContext.User.FindFirst("User").Value.ToUpper();
                if(procesoSeleccionCronograma.ProcesoSeleccionMonitoreoId>0)
                {
                    procesoSeleccionCronograma.UsuarioModificacion = usermodified;
                }
                else
                {
                    procesoSeleccionCronograma.UsuarioCreacion = usermodified;
                }
                
                _response = await _selectionProcessScheduleService.setProcesoSeleccionMonitoreoCronograma(procesoSeleccionCronograma,
                     _settings.Value.DominioFront,
                    _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Data = ex.ToString();
                return Ok(_response);
            }
        }

        /*autor: jflorez
            descripción: trae las actividades por monitoreoid
            impacto: CU 3.1.3*/
        [Route("GetListProcesoSeleccionMonitoreoCronogramaByMonitoreoId")]
        [HttpGet]
        public async Task<ActionResult<List<ProcesoSeleccionCronogramaMonitoreo>>> GetListProcesoSeleccionMonitoreoCronogramaByMonitoreoId(int pProcesoSeleccionId)
        {
            try
            {
                return await _selectionProcessScheduleService.GetListProcesoSeleccionMonitoreoCronogramaByMonitoreoId(pProcesoSeleccionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
