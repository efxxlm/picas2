using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asivamosffie.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdatePoliciesGuaranteesController : ControllerBase
    {
        public readonly IUpdatePoliciesGuaranteesService _updatePoliciesGuaranteesService;

        public UpdatePoliciesGuaranteesController(IUpdatePoliciesGuaranteesService updatePoliciesGuaranteesService)
        {
            _updatePoliciesGuaranteesService = updatePoliciesGuaranteesService;
        }

        [Route("GetContratoPoliza")]
        [HttpGet]
        public async Task<dynamic> GetContratoPoliza([FromQuery] int pContratoPolizaId, bool? pEsNueva = false)
        {
            var respuesta = await _updatePoliciesGuaranteesService.GetContratoPoliza(pContratoPolizaId, pEsNueva);
            return respuesta;
        }

        [Route("GetContratoByNumeroContrato")]
        [HttpGet]
        public async Task<dynamic> GetContratoByNumeroContrato(string pNumeroContrato)
        {
            var respuesta = await _updatePoliciesGuaranteesService.GetContratoByNumeroContrato(pNumeroContrato);
            return respuesta;
        }

        [Route("GetListVActualizacionPolizaYGarantias")]
        [HttpGet]
        public async Task<List<VActualizacionPolizaYgarantias>> GetListVActualizacionPolizaYGarantias()
        {
            return await _updatePoliciesGuaranteesService.GetListVActualizacionPolizaYGarantias();
        }

        [Route("DeleteContratoPolizaActualizacionSeguro")]
        [HttpPost]
        public async Task<IActionResult> DeleteContratoPolizaActualizacionSeguro([FromBody] ContratoPolizaActualizacionSeguro pContratoPolizaActualizacionSeguro)
        {
            Respuesta result = new Respuesta();
            try
            {
                pContratoPolizaActualizacionSeguro.UsuarioCreacion = User.Identity.Name.ToUpper();
                result = await _updatePoliciesGuaranteesService.DeleteContratoPolizaActualizacionSeguro(pContratoPolizaActualizacionSeguro);
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Data = ex;
                return BadRequest(result);
            }
        }

        [Route("DeleteContratoPolizaActualizacion")]
        [HttpPost]
        public async Task<IActionResult> DeleteContratoPolizaActualizacion([FromBody] ContratoPolizaActualizacion pContratoPolizaActualizacion)
        {
            Respuesta result = new Respuesta();
            try
            {
                pContratoPolizaActualizacion.UsuarioCreacion = User.Identity.Name.ToUpper();
                result = await _updatePoliciesGuaranteesService.DeleteContratoPolizaActualizacion(pContratoPolizaActualizacion);
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Data = ex;
                return BadRequest(result);
            }
        }
        [Route("ChangeStatusContratoPolizaActualizacionSeguro")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatusContratoPolizaActualizacionSeguro([FromBody] ContratoPolizaActualizacion pContratoPolizaActualizacion)
        {
            Respuesta result = new Respuesta();
            try
            {
                pContratoPolizaActualizacion.UsuarioCreacion = User.Identity.Name.ToUpper();
                result = await _updatePoliciesGuaranteesService.ChangeStatusContratoPolizaActualizacionSeguro(pContratoPolizaActualizacion);
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Data = ex;
                return BadRequest(result);
            }
        }

        [Route("CreateEditContratoPolizaActualizacion")]
        [HttpPost]
        public async Task<IActionResult> CreateEditContratoPolizaActualizacion([FromBody] ContratoPolizaActualizacion pContratoPolizaActualizacion)
        {
            Respuesta result = new Respuesta();
            try
            {
                pContratoPolizaActualizacion.UsuarioCreacion = User.Identity.Name.ToUpper();
                result = await _updatePoliciesGuaranteesService.CreateEditContratoPolizaActualizacion(pContratoPolizaActualizacion);
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Data = ex;
                return BadRequest(result);
            }
        }

    }
}
