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

        [Route("GetDisponibilidadPresupuestalByID")]
        [HttpGet]
        public async Task<DisponibilidadPresupuestal> GetDisponibilidadPresupuestalByID(int DisponibilidadPresupuestalId)
        {
            var respuesta = await _budgetAvailabilityService.GetDisponibilidadPresupuestalByID(DisponibilidadPresupuestalId);
            return respuesta;
        }

        [Route("ListAdministrativeProject")]
        [HttpGet]
        public async Task<List<DisponibilidadPresupuestalGrilla>> ListAdministrativeProjects()
        {
            // string pUsuarioModifico = HttpContext.User.FindFirst("User").Value;
            var respuesta = await _budgetAvailabilityService.GetListDisponibilidadPresupuestal();
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
                Task<Respuesta> result = _budgetAvailabilityService.SetCancelRegistroPresupuestal(pDisponibilidadPresObservacion);
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
                return File(await _budgetAvailabilityService.GetPDFDDP(id, UsuarioModificacion), "application/pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _budgetAvailabilityService.GetBudgetAvailabilityById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetGridBudgetAvailability")]
        public async Task<IActionResult> GetGridBudgetAvailability(int? DisponibilidadPresupuestalId)
        {
            try
            {
                var result = await _budgetAvailabilityService.GetGridBudgetAvailability(DisponibilidadPresupuestalId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateEditarDP")]
        public async Task<IActionResult> CreateEditarDP([FromBody] DisponibilidadPresupuestal DP)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                DP.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _budgetAvailabilityService.CreateEditarDisponibilidadPresupuestal(DP);
                return Ok(respuesta);
                //
            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        /*autor: jflorez
            descripción: devolver la solicitud por validacion presupuestal
            impacto: CU 3.3.2*/
        [Route("SetReturnValidacionDDP")]
        [HttpPost]
        public async Task<IActionResult> SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                pDisponibilidadPresObservacion.UsuarioCreacion = UsuarioModificacion;
                Task<Respuesta> result = _budgetAvailabilityService.SetReturnValidacionDDP(pDisponibilidadPresObservacion, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /*autor: jflorez
            descripción: rechazar la solicitud por validacion presupuestal
            impacto: CU 3.3.2*/
        [Route("SetRechazarValidacionDDP")]
        [HttpPost]
        public async Task<IActionResult> SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                pDisponibilidadPresObservacion.UsuarioCreacion = UsuarioModificacion;
                Task<Respuesta> result = _budgetAvailabilityService.SetRechazarValidacionDDP(pDisponibilidadPresObservacion, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /*autor: jflorez
            descripción: validar la solicitud por validacion presupuestal
            impacto: CU 3.3.2*/
        [Route("SetValidarValidacionDDP")]
        [HttpPost]
        public async Task<IActionResult> SetValidarValidacionDDP(int id)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                Task<Respuesta> result = _budgetAvailabilityService.SetValidarValidacionDDP(id, UsuarioModificacion, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /*autor: jflorez
            descripción: guarda la definicion de fuentes de financiacion y gasto
            impacto: CU 3.3.2*/
        [Route("CreateFinancialFundingGestion")]
        [HttpPost]
        public async Task<IActionResult> CreateFinancialFundingGestion(GestionFuenteFinanciacion pDisponibilidadPresObservacion)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                pDisponibilidadPresObservacion.UsuarioCreacion = UsuarioModificacion.ToUpper();
                Task<Respuesta> result = _budgetAvailabilityService.CreateFinancialFundingGestion(pDisponibilidadPresObservacion);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /*autor: jflorez
            descripción: elimina la definicion de fuentes de financiacion y gasto
            impacto: CU 3.3.2*/
        [Route("DeleteFinancialFundingGestion")]
        [HttpPost]
        public async Task<IActionResult> DeleteFinancialFundingGestion(int pIdDisponibilidadPresObservacion)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                Task<Respuesta> result = _budgetAvailabilityService.DeleteFinancialFundingGestion(pIdDisponibilidadPresObservacion, UsuarioModificacion);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /*autor: jflorez
            descripción: trae la definicion de fuentes de financiacion y gasto
            impacto: CU 3.3.2*/
        [Route("GetFinancialFundingGestionByDDPP")]
        [HttpGet]
        public async Task<IActionResult> GetFinancialFundingGestionByDDPP(int pIdDisponibilidadPresupuestalProyecto)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                Task<Respuesta> result = _budgetAvailabilityService.GetFinancialFundingGestionByDDPP(pIdDisponibilidadPresupuestalProyecto, UsuarioModificacion);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /*autor: jflorez
            descripción: listado inicial 
            impacto: CU 3.3.4*/
        [Route("GetListGenerarRegistroPresupuestal")]
        [HttpGet]
        public async Task<EstadosDisponibilidad> GetListGenerarRegistroPresupuestal()
        {
            try
            {
                var respuesta = await _budgetAvailabilityService.GetListGenerarRegistroPresupuestal();
                return respuesta;
            }
            catch (Exception ex)
            {
                return new EstadosDisponibilidad();
            }
        }
        /*autor: jflorez
            descripción: cancela la ddp
            impacto: CU 3.3.4*/
        [Route("SetCancelDDR")]
        [HttpPost]
        public async Task<IActionResult> SetCancelarDDR(DisponibilidadPresupuestalObservacion pDisponibilidadPresObservacion)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                pDisponibilidadPresObservacion.UsuarioCreacion = UsuarioModificacion;
                Task<Respuesta> result = _budgetAvailabilityService.SetCancelRegistroPresupuestal(pDisponibilidadPresObservacion);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /*autor: jflorez
            descripción: genera DRP
            impacto: CU 3.3.4*/
        [Route("CreateDRP")]
        [HttpPost]
        public async Task<IActionResult> CreateDRP(int id)
        {

            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                Task<Respuesta> result = _budgetAvailabilityService.CreateDRP(id, UsuarioModificacion, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                object respuesta = await result;
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [Route("GenerateDRP")]
        [HttpGet]
        public async Task<IActionResult> GenerateDRP(int id)
        {
            try
            {
                HttpContext.Connection.RemoteIpAddress.ToString();
                string UsuarioModificacion = HttpContext.User.FindFirst("User").Value;
                //return File(respuesta, "application/octet-stream");
                return File(await _budgetAvailabilityService.GetPDFDRP(id, UsuarioModificacion), "application/pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
