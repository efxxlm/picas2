using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using asivamosffie.services.PostParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestBudgetAvailabilityController : ControllerBase
    {
        private readonly IRequestBudgetAvailabilityService _managementCommitteeReportService;
        private readonly IOptions<AppSettings> _settings;

        public RequestBudgetAvailabilityController(
            IOptions<AppSettings> settings, IRequestBudgetAvailabilityService managementCommitteeReportService)
        {
            _managementCommitteeReportService = managementCommitteeReportService;
            _settings = settings;

        } 

        [Route("GetListAportanteByTipoAportanteByProyectoId")]
        public async Task<dynamic> GetListAportanteByTipoAportanteByProyectoId([FromQuery] int pProyectoId, int pTipoAportanteId)
        {
            return await _managementCommitteeReportService.GetListAportanteByTipoAportanteByProyectoId(pProyectoId, pTipoAportanteId);
        }

        [Route("GetListContatoByNumeroContrato")]
        public async Task<Contrato> GetListContatoByNumeroContrato([FromQuery] string pNumero)
        {
            return await _managementCommitteeReportService.GetListContatoByNumeroContrato(pNumero);
        }

        

        [Route("GetContratoByNumeroContrato")]
        public async Task<Contrato> GetContratoByNumeroContrato([FromQuery] string pNumero)
        {
            return await _managementCommitteeReportService.GetContratoByNumeroContrato(pNumero);
        }

        [Route("GetReuestCommittee")]
        public async Task<IActionResult> GetReuestCommittee()
        {
            try
            {
                var result = await _managementCommitteeReportService.GetReuestCommittee();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetAportantesByProyectoId")]
        public async Task<IActionResult> GetAportantesByProyectoId(int proyectoId)
        {
            try
            {
                var result = await _managementCommitteeReportService.GetAportantesByProyectoId(proyectoId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetAportantesByProyectoAdministrativoId")]
        public async Task<IActionResult> GetAportantesByProyectoAdministrativoId(int proyectoId)
        {
            try
            {
                var result = await _managementCommitteeReportService.GetAportantesByProyectoAdministrativoId(proyectoId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetDDPEspecial")]
        public async Task<IActionResult> GetDDPEspecial()
        {
            try
            {
                var result = await _managementCommitteeReportService.GetDDPEspecial();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetDDPAdministrativa")]
        public async Task<IActionResult> GetDDPAdministrativa()
        {
            try
            {
                var result = await _managementCommitteeReportService.GetDDPAdministrativa();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetDetailInfoAdditionalById")]
        public async Task<IActionResult> GetDetailInfoAdditionalById(int disponibilidadPresupuestalId)
        {
            try
            {
                var result = await _managementCommitteeReportService.GetDetailInfoAdditionalById(disponibilidadPresupuestalId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("GetListCocecutivoProyecto")]
        public async Task<IActionResult> GetListCocecutivoProyecto()
        {
            try
            {
                var result = await _managementCommitteeReportService.GetListCocecutivoProyecto();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("SearchLlaveMEN")]
        public async Task<IActionResult> SearchLlaveMEN(string LlaveMEN)
        {
            try
            {
                var result = await _managementCommitteeReportService.SearchLlaveMEN(LlaveMEN);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Route("CreateOrEditReportProgress")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditReportProgress([FromBody] CompromisoSeguimiento compromisoSeguimiento)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                compromisoSeguimiento.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managementCommitteeReportService.CreateOrEditReportProgress(compromisoSeguimiento);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("CreateUpdateDisponibilidaPresupuestalEspecial")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateDisponibilidaPresupuestalEspecial([FromBody] DisponibilidadPresupuestal pDisponibilidadPresupuestal)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                pDisponibilidadPresupuestal.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managementCommitteeReportService.CreateUpdateDisponibilidaPresupuestalEspecial(pDisponibilidadPresupuestal);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("CreateOrEditProyectoAdministrtivo")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditProyectoAdministrtivo([FromBody] DisponibilidadPresupuestal disponibilidadPresupuestal)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                disponibilidadPresupuestal.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managementCommitteeReportService.CreateOrEditProyectoAdministrtivo(disponibilidadPresupuestal);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("CreateOrEditServiceCosts")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditServiceCosts(DisponibilidadPresupuestal disponibilidadPresupuestal, int proyectoId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                disponibilidadPresupuestal.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managementCommitteeReportService.CreateOrEditServiceCosts(disponibilidadPresupuestal, proyectoId);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("CreateOrEditInfoAdditional")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditInfoAdditional([FromBody] DisponibilidadPresupuestal pDisponibilidad)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                string user = HttpContext.User.FindFirst("User").Value.ToUpper();
                respuesta = await _managementCommitteeReportService.CreateOrEditInfoAdditional(pDisponibilidad, user);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("SendRequest")]
        [HttpPost]
        public async Task<IActionResult> SendRequest(int disponibilidadPresupuestalId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                string user = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managementCommitteeReportService.SendRequest(disponibilidadPresupuestalId,_settings.Value.DominioFront, _settings.Value.MailServer, _settings.Value.MailPort, _settings.Value.EnableSSL, _settings.Value.Password, _settings.Value.Sender);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("EliminarDisponibilidad")]
        [HttpDelete]
        public async Task<IActionResult> EliminarDisponibilidad(int disponibilidadPresupuestalId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                string user = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managementCommitteeReportService.EliminarDisponibilidad(disponibilidadPresupuestalId);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("EliminarDisponibilidad")]
        [HttpPost]
        public async Task<IActionResult> EliminarDisponibilidadPost(int disponibilidadPresupuestalId)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                string user = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managementCommitteeReportService.EliminarDisponibilidad(disponibilidadPresupuestalId);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        [Route("CreateOrEditDDPRequest")]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditDDPRequest([FromBody] DisponibilidadPresupuestal disponibilidadPresupuestal)
        {
            Respuesta respuesta = new Respuesta();
            try
            {

                disponibilidadPresupuestal.UsuarioCreacion = HttpContext.User.FindFirst("User").Value;
                respuesta = await _managementCommitteeReportService.CreateOrEditDDPRequest(disponibilidadPresupuestal);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta.Data = ex.InnerException.ToString();
                return BadRequest(respuesta);
            }
        }

        
        [Route("GetContratos")]
        public async Task<dynamic> GetContratos()
        {
            return await _managementCommitteeReportService.GetContratos();
        }

    }
}
