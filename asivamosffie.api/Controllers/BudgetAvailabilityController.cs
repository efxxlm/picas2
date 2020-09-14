using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetAvailabilityController : ControllerBase
    {

        private readonly IBudgetAvailabilityService _budgetAvailabilityService;
        private readonly IOptions<AppSettings> _settings;

        public BudgetAvailabilityController(IBudgetAvailabilityService budgetAvailabilityService, IOptions<AppSettings> settings)
        {
            _budgetAvailabilityService = budgetAvailabilityService;
            _settings = settings;
        }

        [Route("ListAdministrativeProject")]
        [HttpGet]
        public async Task<List<DisponibilidadPresupuestalGrilla>> ListAdministrativeProjects()
        {
            // string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            var respuesta = await _budgetAvailabilityService.GetListDisponibilidadPresupuestal();
            return respuesta;
        }

        [Route("GetFuenteFinanciacionByIdAportanteId")]
        [HttpGet]
        public async Task<FuenteFinanciacion> GetFuenteFinanciacionByIdAportanteId(int pAportanteId)
        {
            // string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            var respuesta = await _budgetAvailabilityService.GetFuenteFinanciacionByIdAportanteId(pAportanteId);
            return respuesta;
        }

        [Route("GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud")]
        [HttpGet]
        public async Task<List<DisponibilidadPresupuestalGrilla>> GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(string pCodigoEstadoSolicitud)
        {
            var respuesta = await _budgetAvailabilityService.GetListDisponibilidadPresupuestalByCodigoEstadoSolicitud(pCodigoEstadoSolicitud);
            return respuesta;
        }

        /*autor: jflorez
            descripción: objeto para entregar a front los datos ordenados de disponibilidades
            impacto: CU 3.3.3*/
        [Route("GetListGenerarDisponibilidadPresupuestal")]
        [HttpGet]
        public async Task<List<EstadosDisponibilidad>> GetListGenerarDisponibilidadPresupuestal()
        {
            var respuesta = await _budgetAvailabilityService.GetListGenerarDisponibilidadPresupuestal();
            return respuesta;
        }

        /*autor: jflorez
            descripción: cancela la solicitud
            impacto: CU 3.3.3*/
        [Route("SetCancelDDP")]
        [HttpPost]
        public async Task<IActionResult> SetCancelarDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                pDisponibilidadPresObservacion.UsuarioCreacion = UsuarioModificacion;
                Task<Respuesta> result = _budgetAvailabilityService.SetCancelDisponibilidadPresupuestal(pDisponibilidadPresObservacion);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /*autor: jflorez
            descripción: devuelve la solicitud a financiera
            impacto: CU 3.3.3*/
        [Route("SetReturnDDP")]
        [HttpPost]
        public async Task<IActionResult> SetReturnDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                pDisponibilidadPresObservacion.UsuarioCreacion = UsuarioModificacion;
                Task<Respuesta> result = _budgetAvailabilityService.returnDDP(pDisponibilidadPresObservacion);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /*autor: jflorez
            descripción: genera DDP
            impacto: CU 3.3.3*/
        [Route("CreateDDP")]
        [HttpPost]
        public async Task<IActionResult> CreateDDP(int id)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                Task<Respuesta> result = _budgetAvailabilityService.CreateDDP(id, UsuarioModificacion, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [Route("GenerateDDP")]
        [HttpGet]
        public async Task<IActionResult> GenerateDDP(int id)
        {
            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                var respuesta = await _budgetAvailabilityService.GetPDFDDP(id,UsuarioModificacion);
                return File(respuesta, "application/octet-stream");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
