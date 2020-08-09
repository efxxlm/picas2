using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
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

        [Route("ChangeStateProcesoSeleccion")]
        [HttpPut]
        public async Task<Respuesta> ChangeStateProcesoSeleccion([FromBody] ProcesoSeleccion proceso )
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                string usuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _selectionProcessService.ChangeStateProcesoSeleccion(proceso.ProcesoSeleccionId, usuarioCreacion, proceso.EstadoProcesoSeleccionCodigo);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return respuesta;
            }
        }

        [Route("DeleteProcesoSeleccion")]
        [HttpDelete]
        public async Task<Respuesta> DeleteProcesoSeleccion( Int32 pId ){
            Respuesta respuesta = new Respuesta();
            try
            {
                string usuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _selectionProcessService.DeleteProcesoSeleccion(pId, usuarioCreacion);
                return respuesta;
                //
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return respuesta;
            }
        }

        [Route("CreateEditarProcesoSeleccionCronograma")]
        [HttpPost]
        public async Task<Respuesta> CreateEditarProcesoSeleccionCronograma([FromBody] ProcesoSeleccionCronograma procesoSeleccionCronograma){
            Respuesta respuesta = new Respuesta();
            try
            {
                procesoSeleccionCronograma.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _selectionProcessService.CreateEditarProcesoSeleccionCronograma(procesoSeleccionCronograma, false);
                return respuesta;
                //
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return respuesta;
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



         #region Cotizacion

        [Route("GetProcesoSeleccionCotizacion")]
        public async Task<IActionResult> GetProcesoSeleccionCotizacion()
        {
            try
            {
                var result = await _selectionProcessService.GetProcesoSeleccionCotizacion();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        [Route("GetProcesoSeleccionCotizacionById")]
        public async Task<IActionResult> GetProcesoSeleccionCotizacionById(int Id)
        {
            try
            {
                var result = await _selectionProcessService.GetProcesoSeleccionCotizacionById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [Route("GetCotizacionByProcesoSeleccionId")]
        public async Task<IActionResult> GetCotizacionByProcesoSeleccionId(int ProcesoSeleccionId)
        {
            try
            {
                var result = await _selectionProcessService.GetCotizacionByProcesoSeleccionId(ProcesoSeleccionId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateEditarProcesoSeleccionCotizacion")]
        public async Task<IActionResult> CreateEditarProcesoSeleccionCotizacion([FromBody] ProcesoSeleccionCotizacion procesoSeleccionCotizacion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                procesoSeleccionCotizacion.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _selectionProcessService.CreateEditarProcesoSeleccionCotizacion(procesoSeleccionCotizacion);
                return Ok(respuesta);
                //
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


        #endregion


         #region Proceso Seleccion Proponente


        [Route("GetProcesoSeleccionProponenteById")]
        public async Task<IActionResult> GetProcesoSeleccionProponenteById(int Id)
        {
            try
            {
                var result = await _selectionProcessService.GetProcesoSeleccionProponenteById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [Route("GetGridProcesoSeleccionProponente")]
        public async Task<IActionResult> GetGridProcesoSeleccionProponente(int? ProcesoSeleccionId)
        {
            try
            {
                var result = await _selectionProcessService.GetGridProcesoSeleccionProponente(ProcesoSeleccionId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateEditarProcesoSeleccionProponente")]
        public async Task<IActionResult> CreateEditarProcesoSeleccionProponente([FromBody] ProcesoSeleccionProponente procesoSeleccionProponente)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                respuesta = await _selectionProcessService.CreateEditarProcesoSeleccionProponente(procesoSeleccionProponente);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }


        #endregion



        #region Proceso Seleccion Integrante


        [Route("GetProcesoSeleccionIntegranteById")]
        public async Task<IActionResult> GetProcesoSeleccionIntegranteById(int Id)
        {
            try
            {
                var result = await _selectionProcessService.GetProcesoSeleccionIntegranteById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [Route("GetGridProcesoSeleccionIntegrante")]
        public async Task<IActionResult> GetGridProcesoSeleccionIntegrante(int? procesoSeleccionId)
        {
            try
            {
                var result = await _selectionProcessService.GetGridProcesoSeleccionIntegrante(procesoSeleccionId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateEditarProcesoSeleccionIntegrante")]
        public async Task<IActionResult> CreateEditarProcesoSeleccionIntegrante([FromBody] ProcesoSeleccionIntegrante procesoSeleccionIntegrante)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                procesoSeleccionIntegrante.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _selectionProcessService.CreateEditarProcesoSeleccionIntegrante(procesoSeleccionIntegrante);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }
        [Route("SetValidateMassiveLoadElegibilidad")]
        [HttpPost]
        public async Task<IActionResult> SetValidateMassiveLoadElegibilidad(IFormFile file)
        {
            try
            {
                Respuesta respuesta = new Respuesta();

                if (file.Length > 0 && file.FileName.Contains(".xls"))
                {
                    //string strUsuario = "";
                    string strUsuario = HttpContext.User.FindFirst("User").Value;
                    respuesta = await _selectionProcessService.SetValidateCargueMasivo(file, Path.Combine(_settings.Value.DirectoryBase, _settings.Value.DirectoryBaseCargue, _settings.Value.DirectoryBaseOrdeELegibilidad), strUsuario);
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("CreateEditarCronogramaSeguimiento")]
        public async Task<IActionResult> CreateEditarCronogramaSeguimiento([FromBody] CronogramaSeguimiento cronogramaSeguimiento)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                cronogramaSeguimiento.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _selectionProcessService.CreateEditarCronogramaSeguimiento(cronogramaSeguimiento);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        // [Route("GetViewSchedules")]
        // public async Task<IActionResult> GetViewSchedules(int? ProcesoSeleccionCronogramaId)
        // {
        //     try
        //     {
        //         var result = await _selectionProcessService.GetViewSchedules(ProcesoSeleccionCronogramaId);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {

        //         throw ex;
        //     }
        // }











        [Route("UploadMassiveLoadElegibilidad")]
        [HttpPost]
        public async Task<IActionResult> UploadMassiveLoadProjects([FromQuery] string pIdDocument)
        {
            try
            {
                Respuesta respuesta = new Respuesta();
                string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
                respuesta = await _selectionProcessService.UploadMassiveLoadElegibilidad(pIdDocument, pUsuarioModifico);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        #endregion

    }
}
